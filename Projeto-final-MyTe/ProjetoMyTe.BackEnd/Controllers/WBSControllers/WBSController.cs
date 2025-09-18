using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;
using MyTeProject.FrontEnd.Models.WBSModels;

namespace MyTeProject.BackEnd.Controllers.WBSControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class WBSController : CRUDDependencyController<WBS, WBSModel>
    {
        private readonly DbSet<WBSType> _dbSetWBSType;
        private readonly UserManager<AppUser> _userManager;

        public WBSController(AppDbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {
            _dbSetWBSType = dbContext.Set<WBSType>();
            _userManager = userManager;
        }

        [HttpGet("GetWithDependecies/{id}")]
        public override async Task<ActionResult> GetWithDependecies(int id)
        {
            WBS entity =
               await _dbSet
               .Include(e => e.WBSType)
               .Include(e => e.TimeRecords)
               .FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(ConvertEntityToModel(entity));
        }

        [HttpGet("GetWithDependecies/")]
        public override async Task<ActionResult> GetWithDependecies()
        {
            IList<WBS> entities =
                await _dbSet
                .Include(e => e.WBSType)
                .Include(e => e.TimeRecords)
                .Include(e => e.UsersWBS)
                .ThenInclude(f => f.User)
                .ToListAsync();

            AppUser? appUser = await _userManager.GetUserAsync(HttpContext.User);

            IList<WBSModel> models = entities.Select(e => ConvertEntityToModel(e, appUser)).ToList();

            return Ok(models);
        }


        protected override WBSModel ConvertEntityToModel(WBS entity)
        {
            int quantity = entity.TimeRecords == null ? 0 : entity.TimeRecords.Count();
            return new WBSModel
            {
                Id = entity.Id,
                ChargeCode = entity.ChargeCode,
                Description = entity.Description,
                WBSType = entity.WBSType?.Description,
                WBSTypeId = entity.WBSType?.Id,
                QuantityOfTimeRecords = quantity
            };
        }

        protected WBSModel ConvertEntityToModel(WBS entity, AppUser appUser)
        {
            int quantity = entity.TimeRecords == null ? 0 : entity.TimeRecords.Count();
            return new WBSModel
            {
                Id = entity.Id,
                ChargeCode = entity.ChargeCode,
                Description = entity.Description,
                WBSType = entity.WBSType?.Description,
                WBSTypeId = entity.WBSType?.Id,
                UserWBSActive = entity.UsersWBS?.Any(e => e.User.Id == appUser.Id),
                QuantityOfTimeRecords = quantity
            };
        }

        protected override async Task<WBS> ConvertModelToEntity(WBSModel model, WBS? entity = null)
        {
            if (entity == null)
            {
                entity = new WBS();
            }
            entity.ChargeCode = model.ChargeCode;
            entity.Description = model.Description;

            var wbsType = await _dbSetWBSType.FindAsync(model.WBSTypeId);

            if (wbsType == null)
            {
                throw new ArgumentException("wbsTypeId was not found" + model.WBSTypeId);
            }
            entity.WBSType = wbsType;

            return entity;
        }

        protected override async Task PopulateModelStateWithErrors(WBSModel model)
        {
            var chargeCodeExists = await _dbSet.Where(e => e.ChargeCode.Equals(model.ChargeCode) && e.Id != model.Id).ToListAsync();

            if (chargeCodeExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.ChargeCode), $"{model.ChargeCode} is already in use.");
            }
        }
    }
}
