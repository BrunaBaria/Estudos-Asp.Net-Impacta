using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Controllers.Abstract;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;
using MyTeProject.FrontEnd.Models.ExpenseModels;

namespace MyTeProject.BackEnd.Controllers.ExpenseControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class ExpenseController : CRUDDependencyController<Expense, ExpenseModel>
    {
        private readonly DbSet<ExpenseType> _dbSetExpenseType;
        private readonly DbSet<WBS> _dbSetWBS;
        private readonly UserManager<AppUser> _userManager;

        public ExpenseController(AppDbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {
            _dbSetExpenseType = dbContext.Set<ExpenseType>();
            _dbSetWBS = dbContext.Set<WBS>();
            _userManager = userManager;
        }

        public async override Task<ActionResult> GetWithDependecies(int id)
        {
            Expense? entity =
               await _dbSet
               .Include(e => e.User)
               .Include(e => e.WBS)
               .Include(e => e.ExpenseType)
               .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(ConvertEntityToModel(entity));
        }

        public async override Task<ActionResult> GetWithDependecies()
        {
            IList<Expense> entities =
                await _dbSet
                .Include(e => e.User)
                .Include(e => e.WBS)
                .Include(e => e.ExpenseType)
                .ToListAsync();

            IList<ExpenseModel> models = entities.Select(e => ConvertEntityToModel(e)).ToList();

            return Ok(models);
        }

        protected override ExpenseModel ConvertEntityToModel(Expense entity)
        {
            return new ExpenseModel
            {
                Id = entity.Id,
                Date = entity.Date.ToDateTime(TimeOnly.MinValue),
                Value = entity.Value,
                Description = entity.Description,
                WBSId = entity.WBS?.Id,
                WBSDescription = entity.WBS?.Description,
                ExpenseTypeId = entity.ExpenseType?.Id,
                ExpenseTypeDescription = entity.ExpenseType?.Description,
            };
        }

        protected async override Task<Expense> ConvertModelToEntity(ExpenseModel model, Expense? entity = null)
        {
            if (entity == null)
            {
                entity = new Expense();
            }
            entity.Date = DateOnly.FromDateTime(model.Date);
            entity.Value = model.Value;
            entity.Description = model.Description;

            // User
            entity.User = await _userManager.GetUserAsync(HttpContext.User);

            // WBS
            entity.WBS = await _dbSetWBS.FirstOrDefaultAsync(e => e.Id == model.WBSId) ?? throw new ArgumentException("wbsId was not found" + model.WBSId);

            // Expense Type
            entity.ExpenseType = await _dbSetExpenseType.FirstOrDefaultAsync(e => e.Id == model.ExpenseTypeId) ?? throw new ArgumentException("ExpenseTypeId was not found" + model.ExpenseTypeId);

            return entity;
        }
    }
}
