using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Projeto04.AspNet.WebAPI.BackEnd.Models.Entities
{
    public class Cursos
    {
        // definir as props
        [Key]
        public int Curso_Id {  get; set; }
        public string? Curso_Nome { get; set;}
        public double Curso_Mensalidade {  get; set; }
        
        public int Estudante_RA { get; set; }

        // indicar a existencia da relação entre as entities Estudante e Cursos da seguinte forma: definir, aqui, uma prop que possui, como data type, a Entity Estudante - além disso, será necessario fazer uso do atributo [ForeignKey()]

        [ForeignKey("Curso_FK")]
        
        public int Estudante_Id {  get; set; }
        //[JsonIgnore] // desconsiderar - para uso de teste com Swagger - o encapsulamento de dados (em formato JSon) - para uso pleno dos métodos da API
        public Estudante? Estudante { get; set; }
    }
}
