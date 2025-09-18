using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.WBSEntities;
using MyTeProject.FrontEnd.Models.WBSModels;

namespace MyTeProject.BackEnd.Controllers.WBSControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class WBSTypeController : CRUDController<WBSType, WBSTypeModel>
    {
        public WBSTypeController(AppDbContext dbContext) : base(dbContext)
        {
        }


        protected override WBSTypeModel ConvertEntityToModel(WBSType entity)
        {
            return new WBSTypeModel
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }

        protected override async Task<WBSType> ConvertModelToEntity(WBSTypeModel model, WBSType? entity = null)
        {
            if (entity == null)
            {
                entity = new WBSType();
            }
            entity.Description = model.Description;
            return entity;
        }
        protected override async Task PopulateModelStateWithErrors(WBSTypeModel model)
        {
            var descriptionExists = await _dbSet.Where(e => e.Description.Equals(model.Description) && e.Id != model.Id).ToListAsync();

            if (descriptionExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Description), $"{model.Description} is already in use.");
            }
        }
    }
}
