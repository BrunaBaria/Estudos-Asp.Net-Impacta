using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Admin
{
    public class UserController : AuthenticationController
    {
        private IUserService _userService;
        private ILocationService _locationService;
        private IDepartmentService _departmentService;
        private IHiringRegimeService _hiringRegimeService;

        public UserController(IAccountService accountService, IUserService userService, ILocationService locationService, IDepartmentService departmentService, IHiringRegimeService hiringRegimeService) : base([EnumRole.Admin], accountService)
        {
            _userService = userService;
            _locationService = locationService;
            _departmentService = departmentService;
            _hiringRegimeService = hiringRegimeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listOfUsers = await _userService.Get();
                return View(listOfUsers);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(new List<UserModel>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["DepartmentTypes"] = await GetDepartmentTypes();
                ViewData["HiringRegimeTypes"] = await GetHiringRegimeTypes();
                ViewData["LocationTypes"] = await GetLocationTypes();
                ViewData["Roles"] = await GetRolesTypes();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            try
            {
                ViewData["DepartmentTypes"] = await GetDepartmentTypes();
                ViewData["HiringRegimeTypes"] = await GetHiringRegimeTypes();
                ViewData["LocationTypes"] = await GetLocationTypes();
                ViewData["Roles"] = await GetRolesTypes();
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    UserModel user = await _userService.Post(model);
                    ViewData["Success"] = true;
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                UserModel user = await _userService.Get(id);
                ViewData["DepartmentTypes"] = await GetDepartmentTypes(user.DepartmentId);
                ViewData["HiringRegimeTypes"] = await GetHiringRegimeTypes(user.HiringRegimeId);
                ViewData["LocationTypes"] = await GetLocationTypes(user.LocationId);
                ViewData["Roles"] = await GetRolesTypes(user.Role);
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserModel model, int id)
        {

            try
            {
                ViewData["DepartmentTypes"] = await GetDepartmentTypes(model.DepartmentId);
                ViewData["HiringRegimeTypes"] = await GetHiringRegimeTypes(model.HiringRegimeId);
                ViewData["LocationTypes"] = await GetLocationTypes(model.LocationId);
                ViewData["Roles"] = await GetRolesTypes(model.Role);
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    UserModel user = await _userService.Put(model, id);
                    ViewData["Success"] = true;
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var listOfUsers = new List<UserModel>();
            try
            {
                var user = await _userService.Get(id);

                await PopulateModelStateWithErrorsOnDelete(user);

                listOfUsers = await _userService.Get();

                if (ModelState.IsValid)
                {
                    await _userService.Delete(id);
                    return RedirectToAction(nameof(Index));
                }
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(e => e.Errors);
                string errorMessages = string.Join(", ", errors.Select(e => e.ErrorMessage));

                ViewData["Errors"] = errorMessages;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(nameof(Index), listOfUsers);
        }
        protected async Task PopulateModelStateWithErrors(UserModel model)
        {

            var userExists = (await _userService.Get()).Where(e => e.Name.ToUpper().Equals(model.Name?.ToUpper()) && e.Id != model.Id).ToList();

            if (userExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Name), $"{model.Name} is already in use.");

            }

            var emailExists = (await _userService.Get()).Where(e => e.Email.ToUpper().Equals(model.Email?.ToUpper()) && e.Id != model.Id).ToList();

            if (emailExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Email), $"{model.Email} is already in use.");

            }

        }
        protected async Task PopulateModelStateWithErrorsOnDelete(UserModel? model)
        {

            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return;
            }

        }

        private async Task<List<SelectListItem>> GetLocationTypes(int? locationId = null)
        {
            var listOlocations = await _locationService.Get();
            return listOlocations.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.State} - {e.City}",
                Selected = e.Id == locationId
            }).ToList();
        }
        private async Task<List<SelectListItem>> GetHiringRegimeTypes(int? hiringRegimeId = null)
        {
            var listOfhiringRegime = await _hiringRegimeService.Get();
            return listOfhiringRegime.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Description,
                Selected = e.Id == hiringRegimeId
            }).ToList();
        }
        private async Task<List<SelectListItem>> GetDepartmentTypes(int? departmentId = null)
        {
            var listOfdepartment = await _departmentService.Get();
            return listOfdepartment.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name,
                Selected = e.Id == departmentId
            }).ToList();
        }

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
