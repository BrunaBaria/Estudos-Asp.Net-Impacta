using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.BackEnd.Controllers.UserControllers
{
    [Authorize(Policy = "RequiredAdminRole")]
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Properties and Constructor

        private readonly DbSet<Location> _dbSetLocation;
        private readonly DbSet<Department> _dbSetDepartment;
        private readonly DbSet<HiringRegime> _dbSetHiringRegime;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public UserController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, AppDbContext dbContext)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _dbSetLocation = dbContext.Set<Location>();
            _dbSetDepartment = dbContext.Set<Department>();
            _dbSetHiringRegime = dbContext.Set<HiringRegime>();
        }

        #endregion Properties and Constructor

        #region Gets

        [HttpGet("")]
        public async Task<ActionResult> Get()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserModel> models = [];

            // Because of the async method, this operation had to be done this way
            foreach (var user in users)
            {
                models.Add(await ConvertEntityToModel(user));
            }

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userModel = await ConvertEntityToModel(user);

            return Ok(userModel);
        }

        [HttpGet("GetWithDependecies/")]
        public async Task<ActionResult> GetWithDependecies()
        {
            List<AppUser> users = await _userManager.Users
                .Include(e => e.Department)
                .Include(e => e.HiringRegime)
                .Include(e => e.Location)
                .ToListAsync();

            List<UserModel> models = [];

            // Because of the async method, this operation had to be done this way
            foreach (var user in users)
            {
                models.Add(await ConvertEntityToModel(user));
            }

            return Ok(models);
        }

        [HttpGet("GetWithDependecies/{id}")]
        public async Task<ActionResult> GetWithDependecies(int id)
        {
            var user = await _userManager.Users
                .Include(e => e.Department)
                .Include(e => e.HiringRegime)
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userModel = await ConvertEntityToModel(user);

            return Ok(userModel);
        }

        #endregion Gets

        #region Post Put

        [HttpPost("")]
        public async Task<ActionResult> Post(UserModel model)
        {
            await PopulateModelStateWithErrors(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = ConvertModelToEntity(model);

            IdentityResult result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Error inserting the user " + model.Name);
            }

            result = await _userManager.AddToRoleAsync(entity, model.Role.ToString());

            if (!result.Succeeded)
            {
                return BadRequest("Error inserting the user " + model.Name + " in role " + model.Role);
            }

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, UserModel model)
        {
            await PopulateModelStateWithErrors(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _userManager.FindByIdAsync(id.ToString());

            if (entity == null)
            {
                return BadRequest("User with the id " + id + " was not found.");
            }

            entity = ConvertModelToEntity(model, entity);

            entity.PasswordHash = _passwordHasher.HashPassword(entity, model.Password);

            IdentityResult result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
            {
                return BadRequest("Error updating the user " + model.Name);
            }

            result = await UpdateRole(entity, model);

            if (!result.Succeeded)
            {
                return BadRequest("Error updating the user " + model.Name + " in role " + model.Role);
            }

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var searchedEntity =
                await _userManager.FindByIdAsync(id.ToString());

            if (searchedEntity == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(searchedEntity);

            return NoContent();
        }


        private async Task<IdentityResult> UpdateRole(AppUser entity, UserModel model)
        {
            var currentRoles = await _userManager.GetRolesAsync(entity);

            var result = await _userManager.RemoveFromRolesAsync(entity, currentRoles);

            if (!result.Succeeded)
            {
                return result;
            }

            result = await _userManager.AddToRoleAsync(entity, model.Role.ToString());

            return result;

        }

        #endregion Post Put

        #region Convertions

        protected AppUser ConvertModelToEntity(UserModel model, AppUser? entity = null)
        {
            if (entity == null)
            {
                entity = new AppUser();
            }

            entity.UserName = model.Name;
            entity.Email = model.Email;
            entity.Active = model.Active;
            entity.AdmissionDate = DateOnly.FromDateTime(model.AdmissionDate);

            entity.Location = _dbSetLocation.Find(model.LocationId) ?? throw new ArgumentException("Location was not found. Id: " + model.LocationId);
            entity.HiringRegime = _dbSetHiringRegime.Find(model.HiringRegimeId) ?? throw new ArgumentException("Hiring Regime was not found. Id: " + model.HiringRegimeId);
            entity.Department = _dbSetDepartment.Find(model.DepartmentId) ?? throw new ArgumentException("Department was not found. Id: " + model.DepartmentId);

            return entity;
        }

        protected async Task<UserModel> ConvertEntityToModel(AppUser entity)
        {
            var role = (await _userManager.GetRolesAsync(entity)).FirstOrDefault();
            var enumRole = (EnumRole)Enum.Parse(typeof(EnumRole), role);
            var userModel = new UserModel
            {
                Id = entity.Id,
                Name = entity.UserName,
                Email = entity.Email,
                HiringRegime = entity.HiringRegime?.Description,
                HiringRegimeId = entity.HiringRegime?.Id,
                AcceptOvertime = entity.HiringRegime?.AcceptOvertime,
                WorkSchedule = entity.HiringRegime?.WorkSchedule,
                Department = entity.Department?.Name,
                DepartmentId = entity.Department?.Id,
                Location = entity.Location?.State,
                LocationId = entity.Location?.Id,
                Active = entity.Active,
                AdmissionDate = entity.AdmissionDate.ToDateTime(TimeOnly.MinValue),
                Role = enumRole
            };

            return userModel;
        }

        #endregion Convertions

        #region Validation
        protected async Task PopulateModelStateWithErrors(UserModel model)
        {
            var emailExists = await _userManager.Users.Where(e => e.Email.Equals(model.Email) && e.Id != model.Id).ToListAsync();

            if (emailExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Email), $"{model.Email} is already in use.");
            }
        }
        #endregion Validation
    }
}
