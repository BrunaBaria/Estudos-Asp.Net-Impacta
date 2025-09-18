using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Controllers;

public class AccountController : Controller
{
    private IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginData)
    {
        try
        {
            if (ModelState.IsValid)
            {

                if (await _accountService.Login(loginData))
                {
                    ViewData["UserId"] = HttpContext.User;

                    return Redirect("/UserNavigation/TimeRecord");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        ModelState.AddModelError(nameof(loginData.Email), "Usuário ou senha inválidos");

        return View(loginData);
    }


    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Login", "Account");
    }


}
