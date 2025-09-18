using Microsoft.AspNetCore.Mvc;
using Projeto04.Front.APIConsumidor.Models;
using Projeto04.Front.APIConsumidor.Services;
using System.Linq.Expressions;

namespace Projeto04.Front.APIConsumidor.Controllers
{
    public class EstudanteController : Controller
    {
        // definir o elemento referencial para a DI(injeção de dependencia) para as operações do controller
        private EstudanteService _estudanteService;

        // definir o construtor
        public EstudanteController(EstudanteService estudanteService)
        {
            _estudanteService = estudanteService;
        }

        // 1ª tarefa assincrona: Leitura e exibição dos dados, posteriormente, na view
       public async Task<IActionResult> Index()
        {
            try
            {
                var listaEstudantes = await _estudanteService.GetEstudantesAsync();
                return View(listaEstudantes);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Não foi possivel acessar os dados de estudantes.Tente novamente.");
                return View(new List<Estudante>());
            }
        }

        // 2ª tarefa CRUD: Seleção de registro
        public ViewResult GetEstudanteUnico() => View();

        /*
         public async Task<IActionResult> GetEstudanteUnico()
            {
                return View();
            }
         */

        // praticar a sobrecarga para selecionar o registro
        [HttpPost]
        public async Task<IActionResult> GetEstudanteUnico(int id)
        {
            // definir a requisição para a seleção do registro
            var estudante = await _estudanteService.GetEstudanteByIdAsync(id);

            // verificar o valor da variavel
            if (estudante == null)
            {
                return NotFound();
            }
            return View(estudante);
        }

        // 3ª tarefa CRUD: Inserção - Create()
        /* public IActionResult AddEstudante()
         {
             return View();
         }*/
        public ViewResult AddEstudante() => View();

        // sobrecargar do método AddEstudante()
        [HttpPost] // HttpPostAtribute é o recurso de origem da requisição [HttpPost]
        public async Task<IActionResult> AddEstudante(Estudante estudante)
        {
            if (ModelState.IsValid)
            {
                try
                    {
                    await _estudanteService.AddEstudanteAsync(estudante);
                    return RedirectToAction(nameof(Index));
                    }
                catch(Exception ex)
                    {
                    ModelState.AddModelError(string.Empty, "Erro ao criar o registro de estudante");
                    }
            }

            return View(estudante);
        }


        // 4ª tarefa CRUD: Update
        public async Task<IActionResult> UpdateEstudante(int id)
        {
            // estabelecer a requisição para recuperar o registro
            var estudante = await _estudanteService.GetEstudanteByIdAsync(id);

            // verificar se a requisição toruze algum resultado
            if (estudante == null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // praticar a sobrecarga  do método UpdateEstudante
        [HttpPost]
        public async Task<IActionResult> UpdateEstudante(int id, Estudante estudante)
        {
            if (id != estudante.Estudante_Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _estudanteService.UpdateEstudanteAsync(id, estudante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudante);
        }

        // 5ª tarefa CRUD: Excluir - Delete
        [HttpPost]
        public async Task<IActionResult> DeleteEstudante(int id)
        {
            var estudante = await _estudanteService.GetEstudanteByIdAsync(id);
            if (estudante == null)
            {
                return NotFound();
            }

            // caso contrario, a exclusão será executada
            await _estudanteService.DeleteEstudanteAsync(id);

            return RedirectToAction(nameof(Index)); 
        }
    }
}
