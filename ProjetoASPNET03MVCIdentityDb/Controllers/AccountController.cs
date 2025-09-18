using Microsoft.AspNetCore.Mvc;
//uso de diretivas necessárias para os processos de autorização/autenticação de credenciais de acesso à área restrita
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjetoASPNET03MVCIdentityDb.Models;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controller será responsável pela modalidade de autenticação/autorização de usuários para acesso à área restrita da aplicação
    [Authorize] // este atributo faz com que todas as estruturas lógica e instruções relacionadas à esta classe se tornem inacessíveis por qualquer outra instrução sem autorização. Significa que: qualquer instrução que não faça parte desta classe não pode acessar nada que, aqui, está descrito.
    public class AccountController : Controller
    {
        /*
        ===============================================================================================
          1° MOVIMENTO: Configuração/ Disponibilidade dos recursos de acesso à área restrita - LOGIN
        ===============================================================================================
        */

        //1º PASSO: definir dois "auxiliadores" - objetos referenciais - para DI ( Dependence Injection - Injeção de Dependencia)
        private UserManager<AppUser> _gerenciadorDados;
        private SignInManager<AppUser> _gerenciadorAcesso; // enste 2° objeto referencial nada mais é do que um gerenciador de recustos de acesso à áreas restritas de uma aplicação.

        //2° passo: estabelecer o construtor da classe - de forma custumizada;
        //definindo as DIs(Dependence Injections)

        public AccountController(UserManager<AppUser> gerenciadorDados, SignInManager<AppUser> gerenciadorAcesso)
        {
            _gerenciadorDados = gerenciadorDados;
            _gerenciadorAcesso = gerenciadorAcesso;
        }
        /*
        =============================================================================================
         2° Movimento: Definição das Actions de operações para funcionamento do Login
        =============================================================================================
         */
        // 1° passo: estabelecer a action Login(){} para gerar um objeto e fazer acesso ao "endereço/rota" no qual será indicada a necessidade da inserção das credenciais de acesso - 1º estágio da estrutura de login
        [AllowAnonymous] //este atributo/annotation permite o acesso as funcionalidades descritas na action sem a necessidade de autenticação ou autorização prévia
        public IActionResult Login(string returnUrl)
        {
            // praticar a instancias direta do model Login e gerar um objeto do qual se faça uso de suas propriedades
            Login login = new Login();
            // fazer uso do objeto para acessar a prop de url da classe Login
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        // 2° passo: definir, de forma explicita, a tarefa assincrona para o envio de dados e mais um "estagio" da estrutura de login.
        [HttpPost] //atributo/requisição de envio de dados
        [AllowAnonymous] //este atributo/annotation permite o acesso as funcionalidades descritas na action sem a necessidade de autenticação ou autorização prévia
        [ValidateAntiForgeryToken] // este atributo "impede" a atutenticação/autorização desta funcionalidade entre elementos lógicos anônimos - significa que o processo de autentica por elemntos logicos automatizados - robôs - está "barrado"
        public async Task<IActionResult> Login(Login logar)
        {
            //observar as validações do model Login.cs aplicados a view Login.cshtml
            if (ModelState.IsValid)
            {
                // definir a consulta - à base de dados sema partir de sua entity/ representação - será verificado se o valor obtido da prop email é válida
                AppUser consulta = await _gerenciadorDados.FindByEmailAsync(logar.Email); // consulta estabelecida para observar se o email dado pelo usuário - oriundo do parametro logar - existe na base -0 devidamente armazenado.

                // avaliar a consulta
                if (consulta != null)
                {
                    // se a consulta for considerada true - no momento em que ocorre a consulta, o email consultado está logado no sistemas.
                    // se sim, se existe um email que está logado no sistema, o metodo abaixo encerrará a sessão deste e-mail/usuário. Dessa forma o processo de autenticação pode ocorrer em problemas.
                    await _gerenciadorAcesso.SignOutAsync();

                    // fazer uso da classe embarcada SignInResult para operar com o resultado de processo de autenticação do usuário
                    Microsoft.AspNetCore.Identity.SignInResult resultado = await _gerenciadorAcesso.PasswordSignInAsync(consulta, logar.Password, false, false); //este passo é a autenticação propriamente descrita.
                    //aqui, acima temos as seguintes referencias:

                    //uso da prop Email (com refencia à propriedade consulta)- a partir do model Login
                    //uso da prop Password - a partir do model Password observando se ambos - Email e Senha estão em conformidade com o model Login
                    // o 1º false é para indicar que não é necessário persistir a sessão de acesso - quando eu encerrar a aplicação eu quero que o login do usuário caia
                    // o 2º false impede qualquer bloquerio de autenticação/acesso - caso ocorra falha ao tentar autenticar qualquer usuário - não bloqueie usuários por inumeras tentativas.

                    //fazer acesso a var resultado e verificar se o valor atribuido resulta em sucesso - a autenticação 
                    if (resultado.Succeeded)
                    {
                        //abaixo está indicado o endereço da área restrita da aplicação - será uma action que, posteriormente, trabalharemos no HomeController; com sua respectiva view. Neste passo é dado o acesso/autorização - depois da autenticação - à área restrita
                        return Redirect(logar.ReturnUrl ?? "/Home/Private"); //aqui, estamos sendo redirecionados a partir de uma rota/endereço especifico - po isso o uso do método Redirect()

                        //?? este é o operador de coalescencia nula: prop ?? "--uma string--" - se o valor do retorno da operação for diferente de null, o operador de coalescencia retorna o valor refernciado do lado esquerdo do operador, caso contrario, retorna o valor do lado direito
                    }
                }
                ModelState.AddModelError(nameof(logar.Email),"Sua autenticação falhou! Tente novamente.");
            }
            return View(logar);
        }

        // 3° passo: definir uma nova action para que o usuário, UMA VEZ LOGADO, possa sair da área restrita e ser direcionado para uma área pública
        public async Task<IActionResult> Logout()
        {
            await _gerenciadorAcesso.SignOutAsync();
            // indicar a "rota" pela qual o usuário será redirecionado quando escolher sair da área restrita.
            return RedirectToAction("Index","Home"); // aqui, estamos sendo redirecionados para uma action e seu controler
        }

    }
}
