using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Projeto04.AspNet.WebAPI.BackEnd.Models.Entities
{
    // esta classe atua como a entity(entidade representativa)/model - representação da table do DB dentro da aplicação - e aqui serão definidas as props que determinam as regras/formato com as quais os dados serão tratados
    public class Estudante
    {
        [Key] // este atributo indica que a table Estudante - definida no BD - possui um PK(Primary Key) e, aqui, é preciso refernciar - de acordo com a definição dada na table do DB
        // definir as props
        public int Estudante_Id { get; set; }    
        public string? Estudante_Nome {  get; set; }
        public string? Estudante_Sobrenome { get; set; }
        public int Estudante_RA {  get; set; }
        public string? Estudante_Email {  get; set; }
        public int Estudante_Idade {  get; set; }
        public string? Estudante_Fone {  get; set; }
        public string? Estudante_Genero {  get; set; }

        // será necessário definir - observando que existe uma relação entre as tables do Db - uma prop de "navegação" para os cursos que se relacionam com os estudantes
        //[JsonIgnore]
        public ICollection<Cursos>? Curso {  get; set; }
    }
}
