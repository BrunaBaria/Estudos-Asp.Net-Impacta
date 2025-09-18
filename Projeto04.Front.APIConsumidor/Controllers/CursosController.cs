using Microsoft.AspNetCore.Mvc;

namespace Projeto04.Front.APIConsumidor.Controllers
{
    public class CursosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
