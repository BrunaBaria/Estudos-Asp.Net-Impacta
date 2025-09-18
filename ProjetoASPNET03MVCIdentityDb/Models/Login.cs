using System.ComponentModel.DataAnnotations;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe assume o "PAPEL" de ser um instrumento lógico que opera como um "conjunto de propriedades credenciais" - como se fosse um cartão de acesso com informações de um usuário para uma área restrita
    public class Login
    {
        //definir as props necessárias para a operação de autenticação/autorização
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        // por padrão, o AspNetCore vai - SEMPRE - adotar uma URL para o acesso ao "espaço-tela-view" de inserção de credenciais
        //http://localhost:xxxxx/NomeQualquer/Login
        // ao utilizar a prop ReturnUrl estamos dizendo que é possível, se for necessário, customizar a rota para esta área restrita - ou seja, a aplicação pode "fugir" do padrão estabelecido pelo AspNetCore.
    }
}
