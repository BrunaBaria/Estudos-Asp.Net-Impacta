using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // o nome disso é "diretiva"
using Microsoft.AspNetCore.Mvc;
using ProjetoASPNET03MVCIdentityDb.Models;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    //DEFINIR O "PAPEL" Deste CONTROLLER: será responsável pelas operações CRUD do "cadastro" de dados de usuário

    //para este propósito este controller pratica o mecanismo de herança com a superclasse Controller

    //Aqui eu vou dizer que só o Usuário Admin tem acesso para essa área
    [Authorize (Policy="RequiredAdminRole")]
    public class AdminController : Controller
    {
        /*============================================================================================================================
         *  1° MOVIMENTO: DEFINIÇÃO DE ELEMENTOS LÓGICOS REFERENCIAIS E PRÁTICA DE INJEÇÃO DE DEPENDENCIA PARA AS OPERAÇÕES COM DADOS 
         *============================================================================================================================
         */

        // 1° passo: definir uma prop - private - para criar uma elementeo lógico referencial. Neste momento, é importante criar este elemento para que seja usado mo auxílio da manipulação de dados da base - com as quais o controller vai "lidar". Para a definição deste elemento será usada a classe embarcada UserManager<> - oferece recursos de operação com dados de usuário
        // esta classe tem origem no AspNetCore

        private UserManager<AppUser> _gerenciadorDados;

        // 2° passo: definir uma prop - private - para criar uma elementeo lógico referencial.Servirá como referencia para a recuperação/leitura/alteração da senha/password em estrutura hash. Esta prop será definida a partir do recurso de interface embarcada IPasswordHasher

        private IPasswordHasher<AppUser> _senhaCodificada;

        // 3° passo: será a definição da injeção de dependência para este propósito serão usadas as props - definidas acima - de referencia a partir do construtor da classe

        public AdminController(
                UserManager<AppUser> gerenciadorDados, IPasswordHasher<AppUser> senhaCodificada
            ) 
        { 
            // aqui, as props private serão acessadas e, à elas, atribuidos os valores/argumentos dados aos paramentros
            _gerenciadorDados = gerenciadorDados; // _gerenciadorDados(privado) recebe qualquer argumento que for dado à gerenciadorDados(paramento de acesso públic).
            _senhaCodificada = senhaCodificada;
        }


        /*================================================================================================
         * 2° MOVIMENTO: CRIAÇÃO DAS ACTIONS - DEFINIÇÃO DAS OPERAÇÕES CRUD: C: Create(Inserção), R: Read(leitura), U:Update(Atualização/Alteração), D: Delete(Exclusão)
         *
         * Aqui serão usados recursos já definidos dentro do projeto: 
         * 
         *  - AppUser: representação da table do DB. Neste contexto - realação entre AppUser e User - a representação da table será responsável por receber do model User os dados necessários para  as manipulações e, posteriormente os processos de autenticação/ autorização de acesso à área restrita da aplicação.
         *  
         *  - User: é o model que estabelece as "regras/formato" pelos quais os dados serão operados e "relacionados" com o model AppUSer.
         *================================================================================================
         */

        // 1ª operação CRUS - Read (leitura) - action que será responsável pela recuperação/acesso e exibição de dados da base

        /*public IActionResult Index() // esta action é sincrona
        {
            // o que esta action vai retornar?
            return View(_gerenciadorDados.Users);
            //R.: o elemento lógico Users(método get implicito) que foi, acima, referenciado por ser um método get. Dessa forma é possível recuperar os dados da base. É um método exclusivo da classe UserManager<>
        }*/
        public ViewResult Index() => View(_gerenciadorDados.Users);

        // 2ª operação CRUD - Create (Inserção) - action responsável por inserção de dados na base
        // esta 1ª definição da action retornará a view - unicamente

        /*public IActionResult Create()
        {
            return View();
        }
        */
        
        // esta é uma nova forma de definir uma action que trará como resultado a mesma situação indicado acima
        public ViewResult Create() => View(); //quando o metodo for chamado, ele vai retornar o resultado da view create em uma view


        //...continuando a 2ª operação CRUD: sobrecarga da action para que os dados possam ser obtidos e, posteriormente, armazenados 
        // definir o atributo/requisição [HttpPost]
        [HttpPost] // = recebe dados e envia para algum lugar
        // definir - de forma explicita - uma tarefa assincrona para obter os dados e envia-los para a base
        public async Task<IActionResult> Create(User registro)
        {
            // verificar se o ModelState é válido
            if (ModelState.IsValid) // se a avaliação for TRUE
            {
                // Definir um objeto a partir do model/entity AppUser - para, posteriormente, serem praticados os processos de autonticação/autorização de acesso à área restrita da aplicação. Além deste propósito é preciso entender que nesta action será descrito, também, o processo de armazenamento de dados na base.
                AppUser dadosUsuario = new AppUser
                {
                    UserName = registro.Name,
                    Email = registro.Email
                };


                //neste passo, será utilizado - de forma assincrona - um método de criação/inserção de dados na base

                IdentityResult resultadoInsert = await
                    _gerenciadorDados.CreateAsync(dadosUsuario,registro.Password); // aqui está o conjunto de dados com as 3 props definidas no model User

                // é necessário, agora, aninhar um novo if(){} para que os recursos embarcados de sucesso possam indicar o resultado da operação

                if (resultadoInsert.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //estabelecer um loop para investigar /iterar sobre eventuais erros que possam ter ocorrido
                    /*
                    foreach (IdentityError error in resultadoInsert.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    */
                    //chamada do método/função Erros()
                    Erros(resultadoInsert);
                }
            }

            return View(registro);
        }


        // 3ª Operação CRUD - Update ( Atualização/Alteração ): será responsável pela  REinserção de dados na base. Desde que esteja devidamente identificado.
        // para este propósito será necessário disponibilizar o registro.
        public async Task<IActionResult> Update(string idRegistro)
        {
            // definir uma consulta - à base - para a obtenção de um registro. Para este propósito será definida uma propriedade para rceber como valor uma consulta ao registro.
            AppUser buscarRegistro = await _gerenciadorDados.FindByIdAsync(idRegistro);

            // avaliar o resultado da busca e verificar se o registro é realmente, existe.
            if (buscarRegistro != null)
            {
                return View(buscarRegistro);
            }
            else
            {
                return View("Index");
            }
        }
        //...continuando a 3ª operação - Update. Sobrecarga da action/método Update: para que seja agora, possível alterar/atualizar e REenviar os dados para a base
        [HttpPost] //atributo/requisição http que auxilia no envio de dados para a base
        public async Task<IActionResult> Update(string idRegistro, string username, string email, string password)
        {
            // repetir a consulta base
            AppUser buscaRegistro = await _gerenciadorDados.FindByIdAsync(idRegistro);

            //agora é necessário lidar com as props e seus valores para serem alterados e, posteriormente reenviados à base.
            if (buscaRegistro != null)

            {
                // observar o 1º pedaço: o valor da prop Name
                if (!string.IsNullOrEmpty(username))
                {
                    buscaRegistro.UserName = username;
                }
                else
                {
                    ModelState.AddModelError("", "O campo name não pode ser vazio!");
                }
                // observar o primeiro pedaço: o valor da prop Email
                if (!string.IsNullOrEmpty(email))
                {
                    buscaRegistro.Email = email;
                }
                else
                {
                    ModelState.AddModelError("", "O campo email não pode ser vazio!");
                    //observar o 2º pedaço: valor da prop password
                    //=================================================================
                    //esse IF está dentro do ELSE de forma errada. precisa estar fora
                }    //==================================================================
                if (!string.IsNullOrEmpty(password))
                {
                        buscaRegistro.PasswordHash = _senhaCodificada.HashPassword(buscaRegistro, password);
                }
                else
                {
                    ModelState.AddModelError("", "O campo senha/password não pode ser vazio.");
                        // observar o 4º pedaço: consiste em observar e avaliar os dados de nome, email e senha, agora em conjunto. Para que seja possível REenviá-los á base e REarmazená-los - de forma assincrona
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult resultadoOp = await _gerenciadorDados.UpdateAsync(buscaRegistro); //neste ponto, a alteração/atualização ocorre

                    //verificar sucesso da operação
                    if (resultadoOp.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // chamada do método de observação de erros
                        Erros(resultadoOp);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Usuário não encontrado");
            }
            return View(buscaRegistro);
        }

        // 4ª Operação CRUD - Delete ( Exclusão): será responsável pela exclus~]ao de dados da base - desde que o registro esteja devidamente identificado
        [HttpPost]
        // de forma explicita será definida a tarefa assincrona de exclusão de registro
        public async Task<IActionResult> Delete(string idRegistro)
        {
            //definir uma prop buscar o registro na base
            AppUser buscaRegistro = await _gerenciadorDados.FindByIdAsync(idRegistro);

            // verificar o resultado da busca 
            if (buscaRegistro != null)
            {
                IdentityResult resultadoExclusao = await _gerenciadorDados.DeleteAsync(buscaRegistro);

                if (resultadoExclusao.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //NO PROXIMO PASSO VAMOS DEFINIR UMA FUNÇÃO QUE INVESTIGA POTENCIAIS ERROS NA EXCLUSÃO. O NOME DA FUNÇÃO/MÉTODO SERÁ Erros(){}
                    Erros(resultadoExclusao);
                }
            }
            else
            {
                ModelState.AddModelError("","Usuário, infelizmente, não foi encontrado");
            }

            return View("Index",_gerenciadorDados.Users);
        }

        // definir o método Erros()
        private void Erros(IdentityResult ocorrenciaErros)
        {
            //estabelecer um loop para iterar sobre todas as possíveis ocorrencias de eventuais erros na aplicação
            foreach (IdentityError error in ocorrenciaErros.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }

    
}
