using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto04.AspNet.WebAPI.BackEnd.Models;
using Projeto04.AspNet.WebAPI.BackEnd.Models.Entities;

namespace Projeto04.AspNet.WebAPI.BackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        /*
        ===================================================================
           1º movimento: definir os recursos necessários que serão utilizados nas operações de dados da Api
        ===================================================================
        */

        // 1º passo: é necessario praticar a refrencia de instancia da classe MeuDbContext - porque lá está definida a classe que "representa" a tabela do DB para a nossa aplicação (Entity) - esta é a "peça-chave" para que os dados sejam operados pela Api
        private MeuDbContext _dbContext;

        // 2º passo: definir o construtor da classe/controller/api para criar a DI(Dependency Injection)
        public CursosController(MeuDbContext dbContext)
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

        // 2º passo: definir a rota especifica
        [Route("Caneca")]
        // 3º passo: definir a tarefa para a recuperação dos dados - de forma assincrona
        public async Task<ActionResult> RecuperandoDados()
        {
            // criar a consulta que- a partir da entity - pode recuperar todos os dados da base
            var listaCursos = await _dbContext.Cursos.ToListAsync();

            return Ok(listaCursos);
        }

        // 2ª tarefa CRUD: GetOne/id: recuperar um únco registro da base - desde que esteja devidamente armazenado e identificado
        // 1º passo: definir o atributo/requisição necessario
        [HttpGet]
        // 2º passo: definir a rota especifica para acesso ao registro identificado
        [Route("GetOne/{id}")]
        // 3º passo: definir a terafa assincrona
        public async Task<ActionResult> RegistroUnico(int id)
        {
            // definir a consulta a base
            var cursoUnico = await _dbContext.Cursos.FindAsync(id);

            // verificar o resultado da busca
            if (cursoUnico == null)
            {
                return NotFound();
            }

            // retornar um Ok()
            return Ok(cursoUnico);
        }

        // 3ª tarefa CRUD: tarefa de inserção de dados - Create
        //1º passo: definir o atributo/requisição necessario
        [HttpPost]
        // 2º passo: definir a rota
        [Route("criarRegistro")]
        //3º passo: definir a tarefa assincrona
        public async Task<ActionResult> inserindoRegistro(Cursos registroCurso)
        {
            // acessar o contexto de Db e adicionar o registro
            _dbContext.Cursos.Add(registroCurso);

            // de forma assincrona, salvar a alteração/adição
            await _dbContext.SaveChangesAsync();

            // definir a expressão de retorno
            return Ok(registroCurso);
        }


        // 4ª tarefa CRUD: atualizar/alterar registro
        [HttpPut]

        // 2º passo: definir a rota
        [Route("atualizarRegistro/{id}")]

        // 3º passo: definir a tarefa assincrona
        public async Task<ActionResult> AltRegistro([FromRoute] int id, Cursos novoRegCurso)
        {
            // definir uma prop para fazer a consulta a base para encontrar o registro
            var buscandoRegistro = await _dbContext.Cursos.FindAsync(id);

            // verificação da busca
            if (buscandoRegistro == null)
            {
                return NotFound();
            }

            // acessar o registro - com seus valores atuais e altera-los para, posteriormente, salva-los na base de dados
            buscandoRegistro.Curso_Nome = novoRegCurso.Curso_Nome;
            buscandoRegistro.Curso_Mensalidade = novoRegCurso.Curso_Mensalidade;
            buscandoRegistro.Estudante_Id = novoRegCurso.Estudante_Id;
            buscandoRegistro.Estudante_RA = novoRegCurso.Estudante_RA;

            // salvar as alterações - de forma assincrona
            await _dbContext.SaveChangesAsync();

            return Ok(buscandoRegistro);
        }


        // 5ª tarefa CRUD: exclusão de registro

        // 1º passo: estabelecer o atributo/requisição necessario
        [HttpDelete]
        // 2º passo: definir a rota
        [Route("excluindoReg/{id}")]
        //definir a tarefa assincrona
        public async Task<ActionResult> ExcluirReg(int id)
        {
            // consulta para buscar o registro na base
            var excluirRegCurso = await _dbContext.Cursos.FindAsync(id);

            // verificar o resultado da busca
            if (excluirRegCurso == null)
            {
                return NotFound();
            }

            // executar a exclusão
            _dbContext.Remove(excluirRegCurso);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
