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
    public class LocationController : CRUDDependencyController<Location, LocationModel>
    {
        public LocationController(AppDbContext dbContext) : base(dbContext)
        {
        }

        protected override LocationModel ConvertEntityToModel(Location entity)
        {
            int quantity = entity.Users == null ? 0 : entity.Users.Count();
            return new LocationModel
            {
                Id = entity.Id,
                City = entity.City,
                State = entity.State,
                QuantityOfEmployees = quantity
            };
        }

        protected override async Task<Location> ConvertModelToEntity(LocationModel model, Location? entity = null)
        {
            if (entity == null)
            {
                entity = new Location();
            }
            entity.City = model.City;
            entity.State = model.State;

            return entity;
        }
        protected override async Task PopulateModelStateWithErrors(LocationModel model)
        {
            var locationExists = await _dbSet.Where(e => e.State.Equals(model.State) && e.City.Equals(model.City) && e.Id != model.Id).ToListAsync();

            if (locationExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.City), $"{model.State}-{model.City} already exists.");
            }
        }

        [HttpGet("GetWithDependecies/{id}")]
        public override async Task<ActionResult> GetWithDependecies(int id)
        {
            Location entity = await _dbSet
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
            IList<Location> entities =
                await _dbSet
                .Include(e => e.Users)
                .ToListAsync();

            IList<LocationModel> models = entities.Select(e => ConvertEntityToModel(e)).ToList();

            return Ok(models);
        }
    }
}
