using Microsoft.AspNetCore.Mvc;
using Projeto.MVC.ASP.NET_01.Models;

namespace Projeto.MVC.ASP.NET_01.Controllers
{
    // praticando o mecanismo de herança com a classe pai/superclasse Controller
    // é neste momento que a classe criada - PrimeiroController - assume o "papel" de controlador do fluxo de dados da aplicação
    public class PrimeiroController : Controller
    {
        // o que esta classe/controller vai controlar?
        /* R.: Controla o fluxo de dados da aplicação (dados com uma origem; "chegam aqui" - a partir de um determinado "pedaço" da aplicação e, dessa forma, tem seu "percurso desenhado" a partir das instruções que serão descritas no controller)
         * 1 - controla a chegada dos dados
         * 2 - controla as operações com os dados
         * 3 - e o percurso pelo qual os dados circularão
         */


       // 1º passo: agora, para exemplificar a resposta dada acima, será implementado um método que, quando for chamado, retorna uma string
       public string Pompom() // método/ação comum dentro de uma classe
        {
            return "Olá mundo Asp.Net! Meeeeeeeeeeee ajudeeeeeeee!";
        }

        /*
         ================================================================
            Views/Primeiro/Pompom - o Asp.Net Core proporciona para o projeto um mapeamento ideal de todos os seus componentes. É por este motivo que as nomenclaturas do Controller, dos métodos/actions que o compõem e, suas respectivas views precisam, necessariamente, serem obervados com as nomenclaturas semelhantes
         ================================================================
         */

        /*
        // 2º passo: definir um novo método com o IActionResult
        public IActionResult Ola()
        {
            // definir uma propriedade que recebe um valor qualquer e, posteriormente,m este valor deverá ser vinculado à view para que seja exibido
            ViewBag.Message = "Ola mundo Asp.Net. Tô pegando como a coisa funciona!!! hahahahaha";


            return View();
        }*/


        // 3º passo: definir uma nova action para que seja possivel estabelecer a "comunicação" entre Model, View e Controller
        public IActionResult Credenciais()
        {
            // movimento 1: estabelecer, neste momento, a comunicação entre Model  e Controller -> ao praticar a instancia da classe/Model Perfil
            Perfil dados = new Perfil();

            // movimento 2: fazer uso do objeto para acessar as props da classe/Model Perfil{}
            dados.Nome = "Richie Blackmore";
            dados.Idade = 70;
            dados.Endereco = "Inglaterra";



            return View(dados); // aqui, o controller disponibiliza os dados para a view respectiva
        }
    }
}
