using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.FrontEnd.Models.ExpenseModels;

namespace MyTeProject.BackEnd.Controllers.ExpenseControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class ExpenseTypeController : CRUDController<ExpenseType, ExpenseTypeModel>
    {
        public ExpenseTypeController(AppDbContext dbContext) : base(dbContext) { }


        protected override ExpenseTypeModel ConvertEntityToModel(ExpenseType entity)
        {
            return new ExpenseTypeModel
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }

        protected override async Task<ExpenseType> ConvertModelToEntity(ExpenseTypeModel model, ExpenseType? entity = null)
        {
            if (entity == null)
            {
                entity = new ExpenseType();
            }
            entity.Description = model.Description;

            return entity;
        }
        protected override async Task PopulateModelStateWithErrors(ExpenseTypeModel model)
        {
            var descriptionExists = await _dbSet.Where(e => e.Description.Equals(model.Description) && e.Id != model.Id).ToListAsync();

            if (descriptionExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Description), $"{model.Description} is already in use.");
            }
        }
    }
}
