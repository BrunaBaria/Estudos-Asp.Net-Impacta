using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Admin
{
    public class ManagerController : AuthenticationController
    {

        public ManagerController(IAccountService accountService) : base([EnumRole.Admin, EnumRole.Manager], accountService) { }

        #region Reports
        public async Task<IActionResult> GetReport()
        {
            try
            {
                ViewData["Roles"] = await GetRolesTypes();
                return View("Reports");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion Reports

        private async Task<List<SelectListItem>> GetRolesTypes(EnumRole? role = null)
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (EnumRole enumRole in Enum.GetValues(typeof(EnumRole)))
            {
                roles.Add(new SelectListItem
                {
                    Value = enumRole.ToString(),
                    Text = enumRole.ToString(),
                    Selected = enumRole == role
                });
            }

            return roles;
        }
    }
}
