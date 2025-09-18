namespace Projeto04.Front.APIConsumidor.Models
{
    public class Estudante
    {// definir os atributos desta classe - em conformidade com a entity do back-end
        public int Estudante_Id { get; set; }
        public string? Estudante_Nome { get; set; }
        public string? Estudante_Sobrenome { get; set; }
        public int Estudante_RA { get; set; }
        public string? Estudante_Email { get; set; }
        public int Estudante_Idade { get; set; }
        public string? Estudante_Fone { get; set; }
        public string? Estudante_Genero { get; set; }
    }
}
