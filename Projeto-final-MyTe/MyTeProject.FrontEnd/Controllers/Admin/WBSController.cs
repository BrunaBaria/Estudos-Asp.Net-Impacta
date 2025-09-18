using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Models.WBSModels;
using MyTeProject.FrontEnd.Services.Implementation;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Admin
{
    public class WBSController : AuthenticationController
    {
        private IWBSService _wbsService;
        private IWBSTypeService _typeService;

        public WBSController(IAccountService accountService, IWBSService wbsService, IWBSTypeService typeService) : base([EnumRole.Admin], accountService)
        {
            _wbsService = wbsService;
            _typeService = typeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listOfWBS = await _wbsService.Get();
                return View(listOfWBS);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(new List<WBSModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["WBSTypes"] = await GetWbsTypes();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View();
        }

        private async Task<List<SelectListItem>> GetWbsTypes(int? wbsTypeId = null)
        {
            var listOfWbs = await _typeService.Get();
            return listOfWbs.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Description,
                Selected = e.Id == wbsTypeId
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WBSModel model)
        {
            try
            {
                ViewData["WBSTypes"] = await GetWbsTypes();
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    WBSModel wbs = await _wbsService.Post(model);
                    ViewData["Success"] = true;
                    return View(wbs);
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                WBSModel wbs = await _wbsService.Get(id);
                ViewData["WBSTypes"] = await GetWbsTypes(wbs.WBSTypeId);
                return View(wbs);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(WBSModel model, int id)
        {
            try
            {
                await PopulateModelStateWithErrors(model);
                ViewData["WBSTypes"] = await GetWbsTypes();

                if (ModelState.IsValid)
                {

                    WBSModel wbs = await _wbsService.Put(model, id);
                    ViewData["Success"] = true;
                    return View(wbs);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var listOfWbs = new List<WBSModel>();

            try
            {
                var wbs = await _wbsService.Get(id);
                listOfWbs = await _wbsService.Get();
                await PopulateModelStateWithErrorsOnDelete(wbs);

                if (ModelState.IsValid)
                {
                    await _wbsService.Delete(id);
                    return RedirectToAction(nameof(Index));
                }
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(e => e.Errors);
                string errorMessages = string.Join(", ", errors.Select(e => e.ErrorMessage));

                ViewData["Errors"] = errorMessages;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return View(nameof(Index), listOfWbs);
        }

        protected async Task PopulateModelStateWithErrors(WBSModel? model)
        {
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "WBS not found");
                return;
            }

            var chargeCodeExists = (await _wbsService.Get()).Where(e => e.ChargeCode.ToUpper().Equals(model.ChargeCode?.ToUpper()) && e.Id != model.Id).ToList();

            if (chargeCodeExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.ChargeCode), $"{model.ChargeCode} is already in use.");
            }
        }

        protected async Task PopulateModelStateWithErrorsOnDelete(WBSModel? model)
        {

            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "WBS not found");
                return;
            }

            if (model.QuantityOfTimeRecords > 0)
            {

                ModelState.AddModelError(string.Empty, $"WBS cannot be deleted because has {model.QuantityOfTimeRecords} Time Records vinculated");
            }

        }
    }
}
