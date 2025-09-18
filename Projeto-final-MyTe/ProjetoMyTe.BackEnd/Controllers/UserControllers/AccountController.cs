using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Utils.Enums;
using LoginResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MyTeProject.BackEnd.Controllers.UserControllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel login)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AppUser entity = await _userManager.FindByEmailAsync(login.Email);

            if (entity == null || !entity.Active)
            {
                return BadRequest("User invalid!");
            }
            await _signInManager.SignOutAsync();

            LoginResult result = await _signInManager.PasswordSignInAsync(entity, login.Password, false, false);
            if (result.Succeeded)
            {
                return Ok(await ConvertEntityToModel(entity));
            }
            return BadRequest(result);
        }

        [HttpGet("GetAccountMessage")]
        public async Task<ActionResult> GetAccountMessage()
        {
            if (HttpContext.User == null)
            {
                return BadRequest("HttpContext is null");
            }
            AppUser appUser = await _userManager.GetUserAsync(HttpContext.User);

            if (appUser == null)
            {
                return BadRequest("appUser is null");
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            string message = $"Olá {appUser.UserName}. ";

            if (roles.Any())
                message += $"Você possuí acesso como {string.Join(", ", roles)}";
            else
                message += $"Você não possuí nenhum acesso especial";

            return Ok(message);
        }

        [HttpGet]
        public async Task<ActionResult> GetAutheticatedUser()
        {
            if (HttpContext.User == null)
            {
                return BadRequest("HttpContext is null");
            }

            AppUser? appUser = await _userManager.GetUserAsync(HttpContext.User);

            if (appUser == null)
            {
                return BadRequest("appUser is null");
            }

            appUser = await _userManager.Users
                                            .Include(e => e.Department)
                                            .Include(e => e.Location)
                                            .Include(e => e.HiringRegime)
                                            .FirstOrDefaultAsync(e => e.Id == appUser.Id);

            UserModel userModel = await ConvertEntityToModel(appUser);

            return Ok(userModel);
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

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
