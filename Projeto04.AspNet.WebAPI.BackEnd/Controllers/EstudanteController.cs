using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto04.AspNet.WebAPI.BackEnd.Models;
using Projeto04.AspNet.WebAPI.BackEnd.Models.Entities;

// É ESTA A ESTRUTURA DE CÓDIGO - API - QUE PROPORCIONARÁ A IMPLEMENTAÇÃO DAS OPERAÇÕES COM DADOS

namespace Projeto04.AspNet.WebAPI.BackEnd.Controllers
{
    [ApiController] // é o atributo que define o "papel" que a classe EstudanteController assume - ou seja, o "papel" de WebAPI


    [Route("api/[controller]/[action]")] // é o atributo da "rota-padrão" que é acessada para a renderização de action especifica
    // a rota-padrão é desenhada a partir deste contexto: api/EstudanteController


    // pratica do mecanismo de herança com a superclasse ControllerBase: que provê todos os recursos necessarios para o funcionamento da API
    public class EstudanteController : ControllerBase
    {
        /*
         ===================================================================
            1º movimento: definir os recursos necessários que serão utilizados nas operações de dados da Api
         ===================================================================
         */

        // 1º passo: é necessario praticar a refrencia de instancia da classe MeuDbContext - porque lá está definida a classe que "representa" a tabela do DB para a nossa aplicação (Entity) - esta é a "peça-chave" para que os dados sejam operados pela Api
        private MeuDbContext _dbContext;

        // 2º passo: definir o construtor da classe/controller/api para criar a DI(Dependency Injection)
        public EstudanteController(MeuDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        /*
         =====================================================================
            2º movimento: consiste em definir - DE FORMA ASSINCRONA - as tarefas que compostas para API 
         =====================================================================
         */

        // 1ª CRUD - GetAll: recupear todos os dados da base. Essa tarefa será responsavel por fazer uso da injeção de dependencia - acessando a entity para listar todos os dados armazenados

        // 1º passo: definr a tarefa assincrona com o uso do atributo/requisição [HttpGet]  - de forma direta
        [HttpGet]
        // 2º passo: definir uma rota especifica pela qual o acesso a todos os dados serão disponibilizados para a aplicação
        [Route("TodosOsRegistros")] // http://localhost:xxxxx/api/EstudanteController/TodosOsRegistros

        // 3º passo: definição da action de recuperação de dados
        public async Task<ActionResult> Get()
        {
            // criar uma prop para receber como valor - de forma assincorna - todos os registros recuperados da base
            var listaEstudantes = await _dbContext.Estudante.ToListAsync();

            return Ok(listaEstudantes);
        }

        // 2º tarefa CRUD: GetOne/id: recuperar um unico registro da base - desde que esteja devidamente identificado

        // 1º passo: definir a tarefa assincrona com o atributo [HttpGet] - de maneira direta - a partir da base
        [HttpGet]

        // 2º passo: definir a rota especifica
        [Route("GetOne/{id}")]// http://localhost:xxxxx/api/EstudanteController/GetOne/1

        // 3º passo: definir a action para acesso ao registro devidamente selecionado
        public async Task<ActionResult> GetOne(int id)
        {
            // definir uma propriedade para receber como valor  - de forma assicnrona - o registro da base; a partir entity
            var estudanteUnico = await _dbContext.Estudante.FindAsync(id);

            // verificar se o valor da var estudanteUnico existe
            if (estudanteUnico == null)
            {
                return NotFound();
            }

            // se o valor da var for diferente de null será disponibilizado para o front
            return Ok(estudanteUnico);
        }

        // 3º tarefa CRUD - Post/Inserção de dados: AddRegister - inserir dados base. Essa tarefa será responsavel por fazer uso da injeção de dependencia - acessando a entity

        // 1º passo: definir uso do atributo/requisição [HttpPost] - de maneira direta - para que seja possivel inserir o registro
        [HttpPost]

        // 2º passo: definir a rota especifica que o conjunto de dados "percorrerá" até ser inserido na base
        [Route("AddRegister")] // http://localhost:xxxxx/api/EstudanteController/AddRegister

        // 3º passo: definir a tarefa assincrona
        public async Task<ActionResult> Post(Estudante registro)
        {
            // fazer uso da DI para executar a inserção na base
            _dbContext.Estudante.Add(registro);

            // de forma assincrona é preciso INSERIR O REGISTRO.  É de fundamentla importancia indicar que a alteração - inserção em sim - precisa passar pelo processo "salvamento"; e, assim, definitivamente, será armazenado na base
            await _dbContext.SaveChangesAsync();

            // definir a expressão de retorno do método com os dads inseridos na base
            return Ok(registro);
        }


        // 4ª tarefa CRUD: Update -> rota: UpRegister/{id} -> reinserir dados na base. Esta tarefa nserá responsavel por fazer uso da injeção de dependencia, acessando a Entity para alterar e reinserir o registro na base

        // 1º passo: fazer uso do atributo [HttpPut] - de maneria direta - para que seja possivel reinserir o registro na base
        [HttpPut]

        // 2º passo: definir a rota especifica da operação
        [Route("UpRegister/{id}")]
        // 3º passo: definição da tarefa assincrona
        public async Task<ActionResult> PutRegister([FromRoute]int id, Estudante novoRegistro)
        {
            // definir  uma prop para receber como valor - de forma assincrona - um registro da base, acessando a entity, deivdamente identificado com o valor dado ao parametro id
            var buscandoEstudante = await _dbContext.Estudante.FindAsync(id);

            // verificar se o resultado da busca encontrou algum registro
            if (buscandoEstudante == null)
            {
                return NotFound();
            }

            // DEFINIÇÃO DAS OPERAÇÕES DE ALTERAÇÃO DO DADOS ORIGINAIS POR NOVOS DADOS
            buscandoEstudante.Estudante_Nome = novoRegistro.Estudante_Nome;
            buscandoEstudante.Estudante_Sobrenome = novoRegistro.Estudante_Sobrenome;
            buscandoEstudante.Estudante_RA = novoRegistro.Estudante_RA;
            buscandoEstudante.Estudante_Email = novoRegistro.Estudante_Email;
            buscandoEstudante.Estudante_Idade = novoRegistro.Estudante_Idade;
            buscandoEstudante.Estudante_Fone = novoRegistro.Estudante_Fone;
            buscandoEstudante.Estudante_Genero = novoRegistro.Estudante_Genero;

            // confirmar a reinserção dos dados na base -  a partir do uso do método abaixo
            await _dbContext.SaveChangesAsync();

            return Ok(buscandoEstudante);
        }





        // 5ª tarefa CRUD: Delete -> rota: delRegister/{id} - excluir um registro

        // 1º passo: fazer do atributo [HttpDelete] de maneira direta - para excluir o registro
        [HttpDelete]

        // 2º passo: definir a rota especifica
        [Route("delRegister/{id}")]// http://localhost:xxxxx/api/EstudanteController/delRegister/1

        // 3º passo: definir a tarefa assincrona de exclusão do registro
        public async Task<ActionResult> Delete(int id)
        {
            // definir uma prop para receber como valor a consulta à base para identificar e recuperar o registro selecionado
            var excluirRegistro = await _dbContext.Estudante.FindAsync(id);

            // verificar o valor da var excluirRegistro
            if (excluirRegistro == null)
            {
                return NotFound();
            }

            // se o valor da var for valido executamos a exclusão
            _dbContext.Remove(excluirRegistro);

            // "salvar" a alteração que foi feita
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
