using Microsoft.AspNetCore.Mvc;
using Projeto.ASPNET.MVC.CRUD_02.Models;

namespace Projeto.ASPNET.MVC.CRUD_02.Controllers
{
    // este é o "coração" do projeto/aplicação. Aqui, serão imlementadas as operações de dados necessarias - operações estabelecidas a partir da disponibilidade dos dados através da data layer (composta pelo model e pela classe Repository). Posteriormente, os dados serão vinculados na view.

    // para este proposito, a classe ColabController pratica o mecanismo de herança com a superclasse Controller - embarcada/nativa C# .Net
    public class ColabController : Controller
    {

        // 1º tarefa: recuperar os dados do repositorio e lista-los na view. Para este proposito será implementa o método/action Index(). Por que? R.: Porque, no momento em que a aplicação for renderizada e, também este método/action for chamado a execução, a tela/view que surgirá no browser exibirá o resultado da operação implementada abaixo 
        public IActionResult Index()
        {
            // // é necessario indicar à action como acessar o repositorio e, consequentemente, usar a lista de dados que lá esta definida - a partir de sua "capsula-elemento publico".
            return View(Repository.TodosOsColabs);
        }

        // 2ª tarefa: definir a operação da action/método de inserção/criação de dados. Como? R.: Praticar o mecanismo de method overloading (sobrecarga de método) *** é quando um mesmo método/action cumpre - para a aplicação - um numero de tarefas "acumuladas"
        public IActionResult Create()
        {
            // aqui, na declaração do metodo Create(), este somente executa a tarefa de retorna a view referente a esta action
            return View();
        }

        // 2ª tarefa A:
        [HttpPost]// atributo/requisição de envio de dados "de um lado para o outro"
        public IActionResult Create(Colab registroColab)
        {
            // aqui,o método/action sobrecarregado, Create() executará algumas outrar tarefas - além de retorna a view

            //verificar o state de cada um dos dados "inputados" que serão utilizados para o armazenamento no repositorio
            if (ModelState.IsValid)
            {
                // a propriedade ModelState.IsValid verifica se os dados passados para a estrutura de manipualçao estão em "conformidade" com as "regras" estabelecidas no model
                Repository.Inserir(registroColab);
                // acima, esta a ação de inserção de dados na lista!

                return View("Agradecimento", registroColab);

                // acontecerá um redirecionamento do usuario para uma outra view... depois que ocorrer o processo de inserção de dados, será retornada uma view  - para o usuario: view Agradecimento
            }
            else
            {
                // caso algum problema ocorra - a view de inserção permanecerá ativa, carregada no browser
                return View();
            }
        }
    
        
        // 3ª tarefa: definir uma nova action. Seu proposito será atualizar/alterar dados de um registro - devidamente inserido e identificado - da base

        public IActionResult Update(string Identificador)
        {
            // estabelecer uma consulta de seleção de registro à estrutura de armazenamento - para identificar e acessar o registro referente ao valor que será dado ao parametro do método
            Colab consulta = Repository.TodosOsColabs.Where((r) => r.Nome == Identificador).First();
            return View(consulta);
        }

        // 3ª tarefa A: praticando a sobrecarga de método/action Update()
        // agora, será necessario estabelecer as alterações do registro e reenvia-lo para a estrutura de armazenamento
        [HttpPost] // ???????????? por que não o atributo [HttpPut]
        public IActionResult Update(string Identificador, Colab registroAlterado)
        {
            // verificar o state de cada um dos dados "inputados" que serão utilizados para a obtenção dos da view
            if (ModelState.IsValid) // True
            {
                // se os dados forem avaliados como satisfatorios as seguintes operações serão executadas:
                // 1ª movimento: todo o registro será acessado e, consequentemente, suas props; logo na sequencia, a clausula Where() identifica o registro. Posteriormente, compara o valor do parametro em relação a prop de identificação. Assim, o registro pode ser alterado.

                // 1ª alteração: prop Idade
                Repository.TodosOsColabs.Where((r) => r.Nome == Identificador).First().Idade = registroAlterado.Idade;

                // 2ª alteração: prop Salario - a proposta é criar uma consulta generalizada e atribui-la à uma variavel; depois fazer uso desta variavel e associa-la as consultas especificas
                var consulta = Repository.TodosOsColabs.Where((r) => r.Nome == Identificador).First();

                consulta.Salario = registroAlterado.Salario;

                // 3ª alteração: prop Departamento
                consulta.Departamento = registroAlterado.Departamento;

                // 4ª alteração: prop Genero
                consulta.Genero = registroAlterado.Genero;

                // 5ª alteração: prop Nome
                consulta.Nome = registroAlterado.Nome;

                // uma vez que esta tarefa está finalizada - tarefa de alteração de registro - será possivel obervar o resultado da alteração a partir do redirecionamento para a estrutura da view de listagem de dados
                return RedirectToAction("Index");
            }
            return View();
        }

        // 4ª tarefa: definir a action de exclusão de um registro. Para ser excluido o registro precisa, devidamente, identificado
        [HttpPost]
        public IActionResult Delete(string Identificador)
        {
            // definir a consulta de exclusão
            Colab consulta = Repository.TodosOsColabs.Where((r) => r.Nome == Identificador).First();

            // acessar o método Excluir - a partir da classe Repository
            Repository.Excluir(consulta);

            return RedirectToAction("Index");
        }
    
    }
}
