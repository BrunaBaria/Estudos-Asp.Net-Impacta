using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Implementation;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers.Admin
{
    public class LocationController : AuthenticationController
    {
        private ILocationService _locationService;

        public LocationController(IAccountService accountService, ILocationService locationService) : base([EnumRole.Admin], accountService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listOfLocations = await _locationService.Get();
                return View(listOfLocations);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }

            return View(new List<LocationModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocationModel model)
        {
            try
            {
                await PopulateModelStateWithErrors(model);
                if (ModelState.IsValid)
                {
                    LocationModel location = await _locationService.Post(model);
                    ViewData["Success"] = true;
                    return View(location);
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
                LocationModel location = await _locationService.Get(id);
                return View(location);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["Errors"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(LocationModel model, int id)
        {

            try
            {
                await PopulateModelStateWithErrors(model);

                if (ModelState.IsValid)
                {
                    LocationModel location = await _locationService.Put(model, id);
                    ViewData["Success"] = true;
                    return View(location);
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
            var listOfLocations = new List<LocationModel>();
            try
            {
                var location = await _locationService.Get(id);
                await PopulateModelStateWithErrorsOnDelete(location);

                listOfLocations = await _locationService.Get();

                if (ModelState.IsValid)
                {
                    await _locationService.Delete(id);
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

            return View(nameof(Index), listOfLocations);
        }

        protected async Task PopulateModelStateWithErrors(LocationModel model)
        {

            var locationExists = (await _locationService.Get()).Where(e => e.City.ToUpper().Equals(model.City?.ToUpper()) && e.State.ToUpper().Equals(model.State?.ToUpper()) && e.Id != model.Id).ToList();

            if (locationExists.Count != 0)
            {
                ModelState.AddModelError(nameof(model.City), $"{model.State} - {model.City} is already in use.");
            }
        }
        protected async Task PopulateModelStateWithErrorsOnDelete(LocationModel? model)
        {

            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Location not found");
                return;
            }


            if (model.QuantityOfEmployees > 0)
            {

                ModelState.AddModelError(string.Empty, $"Location cannot be deleted because has {model.QuantityOfEmployees} employess");
            }

        }
    }
}
