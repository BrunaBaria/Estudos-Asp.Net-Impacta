using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.TimeRecordEntities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;
using MyTeProject.FrontEnd.Models.TimeRecordModels;

namespace MyTeProject.BackEnd.Controllers.TimeRecordControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class TimeRecordController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;


        public TimeRecordController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("{date}")]
        public async Task<ActionResult> Get(DateTime date)
        {
            var entityAppUser = await GetUser();

            if (entityAppUser == null)
            {
                return BadRequest("Invalid User");
            }

            Fortnight? fortnight = await _appDbContext.Fortnight
                 .Include(e => e.AppUser)
                 .Include(e => e.TimeRecords)
                 .ThenInclude(e => e.WBS)
                 .Where(e => e.AppUser.Id == entityAppUser.Id)
                 .Where(e => e.TimeRecords.Where(e => e.Date == DateOnly.FromDateTime(date)).Any())
                 .FirstOrDefaultAsync();

            if (fortnight == null)
            {
                return Ok(new FortnightModel());
            }
            return Ok(ConvertEntityToModel(fortnight));
        }

        [HttpPost("")]
        public async Task<ActionResult> Post(FortnightModel fortnightModel)
        {
            //1º validar modelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //2° buuscar o user autenticado
            AppUser appUser = await GetUser();
            if (appUser == null)
            {
                return BadRequest("Invalid User");
            }

            //3° buscar o hiring regime do user autenticado(time and overtime)
            var hiringRegime = appUser.HiringRegime;
            //4° for por pela soma das horas da quinzena e validação
            Dictionary<DateOnly, double> dailyAppointments = fortnightModel.TimeRecords
                .Where(n => n.AppointedTime != null && n.WBSId != null)
                .GroupBy(t => t.Date)
                .Select(group => new
                {
                    Date = DateOnly.FromDateTime(group.Key.Value),
                    TotalAppointedTime = group.Sum(t => t.AppointedTime.Value)
                })
                .ToDictionary(item => item.Date, item => item.TotalAppointedTime);

            double timeRecordMin = Convert.ToDouble(hiringRegime.WorkSchedule);
            double timeRecordMax = timeRecordMin + (hiringRegime.AcceptOvertime ? 2 : 0);

            foreach (KeyValuePair<DateOnly, double> day in dailyAppointments)

            {
                if (day.Key.DayOfWeek == DayOfWeek.Sunday || day.Key.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (day.Value != 0)
                    {
                        ModelState.AddModelError(day.Key.ToString(), "It is not possible to post hours on the weekend");
                    }
                }
                else if (day.Value < timeRecordMin)
                {
                    ModelState.AddModelError(day.Key.ToString(), $"It is not possible to post less hours than {timeRecordMin}");
                }
                else if (day.Value > timeRecordMax)
                {
                    ModelState.AddModelError(day.Key.ToString(), $"It is not possible to post more hours than {timeRecordMax}");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //5º salvar a quinzena e timerecord
            //fazer a conversão de model para entidade
            var fortnightEntity = await ConvertModelToEntity(fortnightModel, appUser);

            DateTime date = fortnightModel.TimeRecords.Min(e => e.Date).GetValueOrDefault();

            await Delete(date, appUser);
            _appDbContext.Add(fortnightEntity);

            await _appDbContext.SaveChangesAsync();

            return Ok(fortnightModel);

        }

        private async Task Delete(DateTime date, AppUser appUser)
        {
            Fortnight? fortnightEntity =
                await _appDbContext.Fortnight
                .Include(e => e.TimeRecords)
                .Include(e => e.AppUser)
                .Where(e => e.AppUser.Id == appUser.Id)
                .Where(e => e.TimeRecords.Any(f => f.Date == DateOnly.FromDateTime(date))).FirstOrDefaultAsync();

            if (fortnightEntity != null)
            {
                _appDbContext.Fortnight.Remove(fortnightEntity);
            }
        }

        private async Task<Fortnight> ConvertModelToEntity(FortnightModel fortnightModel, AppUser user, Fortnight? entity = null)
        {
            if (entity == null)
            {
                entity = new Fortnight();
            }
            entity.WorkSchedule = user.HiringRegime.WorkSchedule;
            entity.AppUser = user;

            foreach (var timeRecord in fortnightModel.TimeRecords)
            {
                var timeRecordEntity = await ConvertModelToEntity(timeRecord, entity);
                entity.TimeRecords.Add(timeRecordEntity);
            }
            return entity;
        }

        private async Task<TimeRecord> ConvertModelToEntity(TimeRecordModel timeRecordModel, Fortnight fortnightEntity)
        {
            WBS wbsEntity = await _appDbContext.WBS.FindAsync(timeRecordModel.WBSId);
            /* if (wbsEntity == null)
             {
                 throw new ArgumentException($"This {timeRecordModel.WBSId} wbs is not registered");
             }*/
            TimeRecord timeRecordEntity = new TimeRecord
            {
                Fortnight = fortnightEntity,
                Date = DateOnly.FromDateTime(timeRecordModel.Date.Value),
                AppointedTime = timeRecordModel.AppointedTime,
                WBS = wbsEntity

            };
            return timeRecordEntity;
        }
        private FortnightModel ConvertEntityToModel(Fortnight fortnightEntity)
        {
            var fortnightModel = new FortnightModel
            {
                Id = fortnightEntity.Id,
            };
            foreach (var timeRecordEntity in fortnightEntity.TimeRecords)
            {
                fortnightModel.TimeRecords.Add(ConvertEntityToModel(timeRecordEntity));
            }
            return fortnightModel;
        }
        private TimeRecordModel ConvertEntityToModel(TimeRecord timeRecordEntity)
        {
            bool canAppointToday = timeRecordEntity.Date.DayOfWeek != DayOfWeek.Sunday && timeRecordEntity.Date.DayOfWeek != DayOfWeek.Saturday;
            return new TimeRecordModel
            {
                Id = timeRecordEntity.Id,
                WBSId = timeRecordEntity.WBS?.Id,
                AppointedTime = timeRecordEntity.AppointedTime,
                Date = timeRecordEntity.Date.ToDateTime(TimeOnly.MinValue),
                CanAppointToday = canAppointToday
            };
        }
        private async Task<AppUser?> GetUser()
        {
            if (HttpContext.User == null)
            {
                return null;
            }
            AppUser? appUser = await _userManager.Users
             .Include(u => u.HiringRegime)
             .FirstOrDefaultAsync(u => u.Id.ToString() == _userManager.GetUserId(HttpContext.User));

            return appUser;
        }
    }
}
