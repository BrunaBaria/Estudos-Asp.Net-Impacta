using Microsoft.AspNetCore.Mvc;
using MyTeProject.FrontEnd.Controllers.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Utils.Enums;

namespace MyTeProject.FrontEnd.Controllers
{
    public class AdminController : AuthenticationController
    {
        public AdminController(IAccountService accountService) : base([EnumRole.Admin, EnumRole.Manager], accountService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        /*
        public IActionResult IndexUser()
        {
            return RedirectToAction("Index", "User");
        }
        public IActionResult IndexDepartment()
        {
            return RedirectToAction("Index", "Department");
        }

        public IActionResult IndexWBS()
        {
            return RedirectToAction("Index", "WBS");
        }

        public IActionResult IndexLocation()
        {
            return RedirectToAction("Index", "Location");
        }*/
    }
}