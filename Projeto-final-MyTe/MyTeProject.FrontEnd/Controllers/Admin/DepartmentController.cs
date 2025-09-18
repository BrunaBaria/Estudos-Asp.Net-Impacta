using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Models.Filters;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Admin
{
    public class DepartmentController : AuthenticationController
    {
        private IDepartmentService _departmentService;

        public DepartmentController(IAccountService accountService, IDepartmentService departmentService) : base([EnumRole.Admin], accountService)
        {
            _departmentService = departmentService;
        }


        public async Task<IActionResult> Index(DepartmentFilter? filter = null)
        {
            try
            {
                var listOfDepartments = await _departmentService.Get(filter);
                return View(listOfDepartments);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(new List<DepartmentModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentModel model)
        {

            try
            {
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    DepartmentModel department = await _departmentService.Post(model);
                    ViewData["Success"] = true;
                    return View(department);
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
                DepartmentModel department = await _departmentService.Get(id);
                return View(department);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(DepartmentModel model, int id)
        {

            try
            {
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    DepartmentModel department = await _departmentService.Put(model, id);
                    ViewData["Success"] = true;
                    return View(department);
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
            var listOfDepartments = new List<DepartmentModel>();
            try
            {
                var department = await _departmentService.Get(id);

                PopulateModelStateWithErrorsOnDelete(department);

                listOfDepartments = await _departmentService.Get();

                if (ModelState.IsValid)
                {
                    await _departmentService.Delete(id);
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

            return View(nameof(Index), listOfDepartments);
        }
        protected async Task PopulateModelStateWithErrors(DepartmentModel model)
        {

            var departmentExists = (await _departmentService.Get()).Where(e => e.Name.ToUpper().Equals(model.Name?.ToUpper()) && e.Id != model.Id).ToList();

            if (departmentExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.Name), $"{model.Name} is already in use.");

            }

            var departmentWithThisEmailExists = (await _departmentService.Get()).Where(e => e.ContactEmail.ToUpper().Equals(model.ContactEmail?.ToUpper()) && e.Id != model.Id).ToList();

            if (departmentWithThisEmailExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.ContactEmail), $"{model.ContactEmail} is already in use.");

            }


        }
        protected async Task PopulateModelStateWithErrorsOnDelete(DepartmentModel? model)
        {

            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Department not found");
                return;
            }

            if (model.QuantityOfEmployees > 0)
            {

                ModelState.AddModelError(string.Empty, $"Department cannot be deleted because has {model.QuantityOfEmployees} employess");
                return;
            }
        }
    }
}
