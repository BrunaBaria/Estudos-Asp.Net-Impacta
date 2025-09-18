using System.ComponentModel.DataAnnotations;
namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe assume o "papel" de model domain da aplicação
    //significa que, aqui, serão estabelecidas as regras de manipulação dos dados que circularão pela aplicação - dados referentes a estrutura de validação /autorização de acesso à área restrita da aplicação
    public class User
    {
        // 1° passo: definir 3 propriedades - o propósito é auxiliar na criação de um schema que possa refletir algumas colunas da table do DB
        [Required] //Required é um atributo é uma indicação de Requerimento/obrigatoriedade atribuido à propriedade. Precisa inserir uma diretiva  - using....data..lá em cima)
        public string? Name { get; set; }
        [Required]
        [RegularExpression("[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9])+\\.+[a-zA-Z]{2,6}$")] //REGEX - expressão regular - Os dados abaixo vão precisar seguir o padrão estabelecido celiowiller@gmail.com
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
