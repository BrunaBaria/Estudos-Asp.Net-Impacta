using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTeProject.BackEnd.Entities;

namespace MyTeProject.BackEnd.Controllers.Abstract
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class CRUDDependencyController<TEntity, TModel> : CRUDController<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        protected CRUDDependencyController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet("GetWithDependecies/{id}")]
        public abstract Task<ActionResult> GetWithDependecies(int id);

        [HttpGet("GetWithDependecies")]
        public abstract Task<ActionResult> GetWithDependecies();

    }
}
