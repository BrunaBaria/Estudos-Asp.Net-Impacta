using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Models.ExpenseModels;
using MyTeProject.FrontEnd.Models.TimeRecordModels;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers;

/// <summary>
/// Classe para realizar o controle sobre as telas de navegação do usuário autenticado
/// </summary>
public class UserNavigationController : AuthenticationController
{
    private readonly DateTime minLimitDate = new DateTime(2024, 1, 1);

    private readonly IWBSService _wbsService;

    private readonly ITimeRecordService _timeRecordService;
    public UserNavigationController(IAccountService accountService, IWBSService wbsService, ITimeRecordService timeRecordService) : base([EnumRole.Admin, EnumRole.Manager, EnumRole.User], accountService)
    {
        _wbsService = wbsService;
        _timeRecordService = timeRecordService;
    }
    #region TimeRecord

    public IActionResult Index()
    {
        return RedirectToAction("TimeRecord");

    }
    private FortnightModel ReturnFortnight(DateTime initialDate)
    {
        FortnightModel model = new FortnightModel();
        model.TimeRecords = new List<TimeRecordModel>();
        List<DateTime> fortnightDates = ReturnFortnightDates(initialDate);
        // its a bussiness rule that a user can have only 3 wbs by fortnight

        for (int i = 0; i < 3; i++)
        {

            foreach (var date in fortnightDates)
            {
                bool canAppoint = date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday;
                TimeRecordModel timeRecordModel = new TimeRecordModel
                {
                    Date = date,
                    AppointedTime = null,
                    CanAppointToday = canAppoint
                };
                model.TimeRecords.Add(timeRecordModel);

            }
        }
        return model;
    }

    private static List<DateTime> ReturnFortnightDates(DateTime initialDate)
    {
        List<DateTime> fortnightDates = new List<DateTime>();

        for (int i = 0; i < 15; i++)
        {
            var currentDate = initialDate.AddDays(i);

            if (currentDate.Month != initialDate.Month)
            {
                break;
            }
            fortnightDates.Add(currentDate);

        }

        return fortnightDates;
    }

    /// <summary>
    /// Sends the user to the visualization of the TimeRecord View
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> TimeRecord(DateTime? date = null)
    {
        if (date == null)
        {
            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }
        else if (date.Value < minLimitDate)
        {
            date = minLimitDate;
        }
        if (DateTime.Now.Day > 15)
        {
            date = date.Value.AddDays(15);
        }
        FortnightModel fortnight = await _timeRecordService.Get(date.Value);
        if (fortnight == null || fortnight.TimeRecords == null || fortnight.TimeRecords.Count == 0)
        {
            fortnight = ReturnFortnight(date.Value);
        }
        ViewData["ListOfNavigationsDates"] = GetNavigationDates(date.Value);
        ViewData["ListOfWBS"] = await GetWBS();
        ViewData["UserAuthenticated"] = authenticatedUser;
        return View(fortnight);
    }

    private async Task<List<SelectListItem>> GetWBS(int? wbsId = null)
    {
        var listOfWbs = await _wbsService.Get();
        return listOfWbs.Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = $"{e.Description} [{e.ChargeCode}]",
            Selected = e.Id == wbsId
        }).ToList();

    }

    private List<SelectListItem> GetNavigationDates(DateTime date)
    {
        List<SelectListItem> listOfNavigationDates = new List<SelectListItem>();

        DateTime currentDate;

        for (int i = -3; i <= 3; i++)
        {
            currentDate = date.AddMonths(i);

            if (currentDate >= minLimitDate)
            {
                listOfNavigationDates.Add(new SelectListItem
                {
                    Value = currentDate.ToShortDateString(),
                    Text = currentDate.ToShortDateString(),
                    Selected = currentDate == date
                });
            }
        }
        return listOfNavigationDates;
    }

    /// <summary>
    /// Receives the information and returns the success of the operation using ViewData
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> TimeRecord(FortnightModel model)
    {
        try
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (DateTime.Now.Day > 15)
            {
                date = date.AddDays(15);
            }
            ViewData["ListOfNavigationsDates"] = GetNavigationDates(date);
            ViewData["ListOfWBS"] = await GetWBS();
            ViewData["UserAuthenticated"] = authenticatedUser;
            await PopulateModelStateWithErrors(model);
            if (ModelState.IsValid)
            {
                await _timeRecordService.Post(model);
                ViewData["Success"] = true;
                return View(nameof(TimeRecord), model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        IEnumerable<ModelError> errors = ModelState.Values.SelectMany(e => e.Errors);
        string errorMessages = string.Join(", ", errors.Select(e => e.ErrorMessage));

        ViewData["Errors"] = errorMessages;

        return View(nameof(TimeRecord), model);

    }

    protected async Task PopulateModelStateWithErrors(FortnightModel model)
    {
        // nao pode ser nulo
        if (model == null || model.TimeRecords == null)
        {
            ModelState.AddModelError(string.Empty, "You must fill at least one row of the table");
            return;
        }
        // onde o id da wbs for nulo remover a soma
        if (model.TimeRecords.Any(t => t.WBSId == null && t.AppointedTime > 0))
        {
            ModelState.AddModelError(string.Empty, "An appointed time must have a WBS");
            return;
        }

        var filteredTimeRecords = model.TimeRecords.Where(t => t.WBSId != null && t.AppointedTime > 0).ToList();

        if (filteredTimeRecords.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "You must fill at least one row of the table");
            return;
        }
        // soma diaria está dentro do esperado

        Dictionary<DateOnly, double> dailyAppointments = filteredTimeRecords
                .GroupBy(t => t.Date)
                .Select(group => new
                {
                    Date = DateOnly.FromDateTime(group.Key.Value),
                    TotalAppointedTime = group.Sum(t => t.AppointedTime.Value)
                })
                .ToDictionary(item => item.Date, item => item.TotalAppointedTime);

        double timeRecordMin = Convert.ToDouble(authenticatedUser.WorkSchedule);
        double timeRecordMax = timeRecordMin + (authenticatedUser.AcceptOvertime == true ? 2 : 0);

        foreach (KeyValuePair<DateOnly, double> day in dailyAppointments)
        {
            if (day.Key.DayOfWeek == DayOfWeek.Sunday || day.Key.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day.Value != 0)
                {
                    ModelState.AddModelError(day.Key.ToString(), "It is not possible to post hours on the weekend");
                    return;
                }
            }
            else if (day.Value < timeRecordMin)
            {
                ModelState.AddModelError(day.Key.ToString(), $"It is not possible to post less hours than {timeRecordMin}");
                return;
            }
            else if (day.Value > timeRecordMax)
            {
                ModelState.AddModelError(day.Key.ToString(), $"It is not possible to post more hours than {timeRecordMax}");
                return;
            }

        }

        double sumOfAppointedTime = model.TimeRecords.Sum(s => s.AppointedTime).Value;
        DateTime initialDate = model.TimeRecords.Min(t => t.Date).GetValueOrDefault();
        List<DateTime> fortnightDates = ReturnFortnightDates(initialDate).Where(d => d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday).ToList();

        if (fortnightDates.Count * timeRecordMin > sumOfAppointedTime)
        {
            ModelState.AddModelError(string.Empty, "Fill all days of the week");
            return;
        }
        if (fortnightDates.Count * timeRecordMax < sumOfAppointedTime)
        {
            ModelState.AddModelError(string.Empty, "You appointed more hours than you can");
            return;
        }

        // validando se está faltando lançar horas para algum dia da quinzena
        //
        //DateOnly initialDate = model.TimeRecords.Min(t => DateOnly.Parse(t.Date.ToString()));
        //for (int i = 0; i < 15; i++)
        //{
        //    DateOnly currentDate = initialDate.AddDays(i);

        //    if (currentDate.Month != initialDate.Month)
        //    {
        //        break;
        //    }

        //    if (currentDate.DayOfWeek != DayOfWeek.Sunday && currentDate.DayOfWeek != DayOfWeek.Saturday)
        //    {
        //        if (!dailyAppointments.Keys.Contains(currentDate))
        //        {
        //            ModelState.AddModelError(string.Empty, "You need to fill all days before sending the Fortnight");
        //            return;
        //        }
        //    }
        //}

    }

    #endregion TimeRecord

    #region ChargeCode
    /// <summary>
    /// Sends the user to the visualization of the ChargeCode View
    /// </summary>
    /// <returns></returns>
    public IActionResult ChargeCode()
    {
        return View();
    }

    /// <summary>
    /// Receives the information and returns the success of the operation using ViewData
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult ChargeCode(UserModel data)
    {
        ViewData["Success"] = true;
        return View();
    }

    #endregion ChargeCode

    #region Expenses

    /// <summary>
    /// Sends the user to the visualization of the Expenses View
    /// </summary>
    /// <returns>Returns expense view</returns>
    public IActionResult Expenses()
    {
        return View();
    }

    /// <summary>
    /// Receives the information and returns the success of the operation using ViewData
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Expenses(ExpenseModel data)
    {
        ViewData["Success"] = true;
        return View();
    }

    #endregion Expenses

}
