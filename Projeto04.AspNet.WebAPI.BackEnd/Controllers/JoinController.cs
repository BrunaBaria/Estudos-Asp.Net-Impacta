using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto04.AspNet.WebAPI.BackEnd.Models.Entities;
using Projeto04.AspNet.WebAPI.BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Projeto04.AspNet.WebAPI.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JoinController : ControllerBase
    {
        private readonly MeuDbContext _dbContext;

        public JoinController(MeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // INICIO DAS OPERAÇÕES DE RELACIONAMENTEO ENTRE AS ENTITIES D APLICAÇÃO

        // 1ª TAREFA JOIN: recuperar todos os Cursos armazenados na base. No entanto, este cursos, agora, serão "associados" ao registros de estudantes que, neles, estão inscritos.

        // 1º passo: definir o atributo de requisição adequado - [HttpGet]
        [HttpGet]
        // 2º passo: definir a rota adequada para a aoperação
        [Route("GetJoinTodosCursos")]

        // 3º passo: definir a tarefa assincrona para a execução da operação
        public async Task<ActionResult> GetJoinTodos()
        {
            // 4º passo: criar uma prop para receber - de forma assincrona - como valor, todos os registros da base fazendo acesso as entities relacionadas entre si
            var cursosComEstudantes = await _dbContext.Cursos.Include(cs => cs.Estudante).ToListAsync();

            return Ok(cursosComEstudantes);
        }

        // 2ª TAREFA JOIN: recuperar todos os Estudantes armazenados na base. No entanto, este estudantes, agora, serão "associados" ao registros de curso que, neles, estão inscritos.

        // 1º passo: definir o atributo de requisição adequado - [HttpGet]
        [HttpGet]


        [Route("GetJoinTodosOsEstudantes")]

        public async Task<ActionResult<IEnumerable<Estudante>>> GetEstudantesComCursos()
        {
            var estudantesComCursos = await _dbContext.Estudante
                .Include(e => e.Curso).ToListAsync();


            return Ok(estudantesComCursos);

            /*
            // uma outra abordagem
            var estudantesComCursos = await _dbContext.Estudante.Select(
                    juntando => new
                    {
                        Estudante = juntando, Cursos = _dbContext.Curso.Where(juntandoComCurso => juntandoComCurso.EstudanteId == juntando.EstudanteId).ToList()
                    }
                ).ToListAsync();
            return Ok(estudantesComCursos);*/

        }

        // 3ª TAREFA JOIN: recuperar um unico registro - a partir da entity Estudante - armazenado na base. Além disso, ao recuperar este registro especifico de estudnate é de fundamental importancia trazer, "associados"  ao registro, os cursos que fazem parte deste perfil de estudante - cursos nos quais ele, estudante, está inscrito.

        // 1º passo: definir o atributo de requisição adequado - [HttpGet]
        [HttpGet]
        [Route("GetOneJoinEstudanteId/{id}")]
        public async Task<ActionResult<Estudante>> GetOneJoinEstudanteId(int id)
        {
            var estudanteComCursos = await _dbContext.Estudante
                .Include(e => e.Curso) // Carrega os pedidos relacionados ao cliente
                .FirstOrDefaultAsync(e => e.Estudante_Id == id);

            if (estudanteComCursos == null)
            {
                return NotFound(); // Retorna 404 se o cliente não for encontrado.
            }

            return Ok(estudanteComCursos);
        }

        [HttpGet]
        [Route("buscaParametro")]
        /// /buscaParametro?termo=+termo
        public async Task<ActionResult> buscaFiltrada(string termo)
        {
            var resultadoFiltrado = await _dbContext.Cursos.Where(t => t.Curso_Nome.Contains(termo)).Include(t => t.Estudante).ToListAsync();

            return Ok(resultadoFiltrado);
        }
    }
}
