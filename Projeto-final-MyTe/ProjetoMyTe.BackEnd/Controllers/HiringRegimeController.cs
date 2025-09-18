using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.FrontEnd.Models.UserModels;

namespace MyTeProject.BackEnd.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class HiringRegimeController : CRUDController<HiringRegime, HiringRegimeModel>
    {
        public HiringRegimeController(AppDbContext dbContext) : base(dbContext)
        {

        }

        protected override HiringRegimeModel ConvertEntityToModel(HiringRegime entity)
        {
            return new HiringRegimeModel
            {
                Id = entity.Id,
                Description = entity.Description,
                AcceptOvertime = entity.AcceptOvertime,
                WorkSchedule = entity.WorkSchedule
            };
        }

        protected override async Task<HiringRegime> ConvertModelToEntity(HiringRegimeModel model, HiringRegime? entity = null)
        {
            if (entity == null)
            {
                entity = new HiringRegime();
            }
            entity.Description = model.Description;
            entity.AcceptOvertime = model.AcceptOvertime;
            entity.WorkSchedule = model.WorkSchedule;

            return entity;
        }
        protected override async Task PopulateModelStateWithErrors(HiringRegimeModel model)
        {
            var descriptionExists = await _dbSet.Where(e => e.Description.Equals(model.Description) && e.Id != model.Id).ToListAsync();

            if (descriptionExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Description), $"{model.Description} is already in use.");
            }
        }
    }
}
