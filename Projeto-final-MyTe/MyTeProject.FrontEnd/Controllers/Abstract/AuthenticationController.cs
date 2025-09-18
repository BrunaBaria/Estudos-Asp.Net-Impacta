using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Abstract
{
    public abstract class AuthenticationController : Controller
    {
        protected readonly List<EnumRole?> _roles;
        protected readonly IAccountService _accountService;
        protected UserModel authenticatedUser;

        public AuthenticationController(List<EnumRole?> roles, IAccountService accountService)
        {
            _roles = roles;
            _accountService = accountService;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                authenticatedUser = await _accountService.Get();

                if (authenticatedUser == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
                else if (!_roles.Contains(authenticatedUser.Role))
                {
                    context.Result = new RedirectToActionResult("TimeRecord", "UserNavigation", null);
                    ViewBag.User = authenticatedUser;
                }
                else
                {
                    ViewBag.User = authenticatedUser;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
