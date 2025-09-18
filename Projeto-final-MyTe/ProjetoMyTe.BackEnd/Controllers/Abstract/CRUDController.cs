using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities;

namespace MyTeProject.BackEnd.Controllers.Abstract
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public abstract class CRUDController<TEntity, TModel> : ControllerBase
        where TEntity : class
        where TModel : class
    {

        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public CRUDController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            IList<TModel> models =
                (await _dbSet.ToListAsync()).Select(e => ConvertEntityToModel(e)).ToList();
            return Ok(models);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> Get(int id)
        {
            TEntity entity =
                await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(ConvertEntityToModel(entity));
        }

        [HttpPost("")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> Post(TModel model)
        {
            await PopulateModelStateWithErrors(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbSet.Add(await ConvertModelToEntity(model));
            await _dbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> Put([FromRoute] int id, TModel model)
        {
            await PopulateModelStateWithErrors(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity searchedEntity =
                await _dbSet.FindAsync(id);

            if (searchedEntity == null)
            {
                return NotFound();
            }

            await ConvertModelToEntity(model, searchedEntity);

            await _dbContext.SaveChangesAsync();

            return Ok(model);

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> Delete(int id)
        {
            TEntity searchedEntity =
                await _dbSet.FindAsync(id);

            if (searchedEntity == null)
            {
                return NotFound();
            }

            _dbSet.Remove(searchedEntity);

            await _dbContext.SaveChangesAsync();

            return NoContent();

        }

        protected abstract Task<TEntity> ConvertModelToEntity(TModel model, TEntity? entity = null);

        protected abstract TModel ConvertEntityToModel(TEntity entity);

        protected async virtual Task PopulateModelStateWithErrors(TModel model)
        {

        }
    }
}
