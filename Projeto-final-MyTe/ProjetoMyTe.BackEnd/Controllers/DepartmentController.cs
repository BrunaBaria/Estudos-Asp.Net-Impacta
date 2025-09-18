using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.FrontEnd.Models.Filters;
using MyTeProject.FrontEnd.Models.UserModels;

namespace MyTeProject.BackEnd.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class DepartmentController : CRUDDependencyController<Department, DepartmentModel>
    {

        public DepartmentController(AppDbContext dbContext) : base(dbContext) { }



        protected override DepartmentModel ConvertEntityToModel(Department entity)
        {
            int quantity = entity.Users == null ? 0 : entity.Users.Count();
            return new DepartmentModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ContactEmail = entity.ContactEmail,
                QuantityOfEmployees = quantity
            };
        }

        protected override async Task<Department> ConvertModelToEntity(DepartmentModel model, Department? entity = null)
        {
            if (entity == null)
            {
                entity = new Department();
            }
            entity.Name = model.Name;
            entity.ContactEmail = model.ContactEmail;
            return entity;
        }
        protected override async Task PopulateModelStateWithErrors(DepartmentModel model)
        {
            var nameExists = await _dbSet.Where(e => e.Name.Equals(model.Name) && e.Id != model.Id).ToListAsync();

            if (nameExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Name), $"{model.Name} is already in use.");
            }
        }

        [HttpGet("GetWithDependecies/{id}")]
        public override async Task<ActionResult> GetWithDependecies(int id)
        {
            Department entity =
               await _dbSet
               .Include(e => e.Users)
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
            IList<Department> entities =
                await _dbSet
            .Include(e => e.Users)
            .ToListAsync();

            IList<DepartmentModel> models = entities.Select(e => ConvertEntityToModel(e)).ToList();

            return Ok(models);
        }

        [HttpPost("GetWithFilters/")]
        public async Task<ActionResult> GetWithFilters([FromBody] DepartmentFilter filter)
        {
            IList<Department> entities =
                await _dbSet
            .Include(e => e.Users)
            .Where(e => (e.Id == filter.Id) || filter.Id == null || filter.Id == 0)
            .Where(e => filter.Name == null || (e.Name.Contains(filter.Name)))
            .ToListAsync();

            IList<DepartmentModel> models = entities.Select(e => ConvertEntityToModel(e)).ToList();

            return Ok(models);
        }
    }
}
