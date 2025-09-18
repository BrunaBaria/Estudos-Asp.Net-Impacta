using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoASPNET03MVCIdentityDb.Models;
using System.Diagnostics;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controle será responsável por exercer controle e influencia sobre a área restrita da aplicação
    [Authorize(Policy = "RequiredManagerRole")]
    public class HomeController : Controller
    {
        // objeto referencial definido a partir da interface ILogger. Seu propósito é auxiliar na obtenção de informações de log a respeito do comportamento da aplicação
        private readonly ILogger<HomeController> _logger;

        // 1° passo: definir um auxiliador - objeto referncial - a partir da classe UserManager
        private UserManager<AppUser> _gerenciadorDados;

        // este o construtor da classe onde são definidas as DIs
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> gerenciadorDados)
        {
            _logger = logger;

            //definir a DI
            _gerenciadorDados = gerenciadorDados;
        }

        // definição das actions que se relacionam com este controller
        public IActionResult Index()
        {
            return View();
        }

        [Authorize] //você pode autorizar o acesso a página de acordo com os perfirs
        public IActionResult Privacy()
        {
            return View();
        }

        // Importante para o MyTE
        // 3° passo: criar uma nova action para "controlar" as operações em relação a view que será definida para a área restrita da aplicação
        [Authorize] //o uso do atributo [Authorize] define que a action precisa ser acessada por um contexto de autenticação e autorização dadas à um determinado conjunto de dados - credenciais de acesso.
        public async Task<IActionResult> Private()
        {
            AppUser consultaUser = await _gerenciadorDados.GetUserAsync(HttpContext.User);
            //uso do recusro HttoContext -> metodo get implicito
            //uso do recurso User -> método set implicito

            //criar uma nova prop para receber como valor uma mensagem de boas-vindas associados ao nome do usuário
            string mensagem = "Olá " + consultaUser.UserName + " você está na área restrita da aplicação";
            return View((object)mensagem); // tranformei minha propriedade mensagem em um objeto usando esse casting((object)mensagem)para poder instanciá-la na minha view.
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
