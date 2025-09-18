using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoASPNET03MVCIdentityDb.Models;
using System.Diagnostics;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controle ser� respons�vel por exercer controle e influencia sobre a �rea restrita da aplica��o
    [Authorize(Policy = "RequiredManagerRole")]
    public class HomeController : Controller
    {
        // objeto referencial definido a partir da interface ILogger. Seu prop�sito � auxiliar na obten��o de informa��es de log a respeito do comportamento da aplica��o
        private readonly ILogger<HomeController> _logger;

        // 1� passo: definir um auxiliador - objeto referncial - a partir da classe UserManager
        private UserManager<AppUser> _gerenciadorDados;

        // este o construtor da classe onde s�o definidas as DIs
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> gerenciadorDados)
        {
            _logger = logger;

            //definir a DI
            _gerenciadorDados = gerenciadorDados;
        }

        // defini��o das actions que se relacionam com este controller
        public IActionResult Index()
        {
            return View();
        }

        [Authorize] //voc� pode autorizar o acesso a p�gina de acordo com os perfirs
        public IActionResult Privacy()
        {
            return View();
        }

        // Importante para o MyTE
        // 3� passo: criar uma nova action para "controlar" as opera��es em rela��o a view que ser� definida para a �rea restrita da aplica��o
        [Authorize] //o uso do atributo [Authorize] define que a action precisa ser acessada por um contexto de autentica��o e autoriza��o dadas � um determinado conjunto de dados - credenciais de acesso.
        public async Task<IActionResult> Private()
        {
            AppUser consultaUser = await _gerenciadorDados.GetUserAsync(HttpContext.User);
            //uso do recusro HttoContext -> metodo get implicito
            //uso do recurso User -> m�todo set implicito

            //criar uma nova prop para receber como valor uma mensagem de boas-vindas associados ao nome do usu�rio
            string mensagem = "Ol� " + consultaUser.UserName + " voc� est� na �rea restrita da aplica��o";
            return View((object)mensagem); // tranformei minha propriedade mensagem em um objeto usando esse casting((object)mensagem)para poder instanci�-la na minha view.
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
