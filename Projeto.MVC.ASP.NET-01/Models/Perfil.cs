namespace Projeto.MVC.ASP.NET_01.Models
{
    // esta classe assume o "papel" de model domain da aplicação. Isso significa que: as "regras/formato" com as quais os dados serão manipulados pela aplicação ficam estabelecidas aqui
    public class Perfil
    {
        /*
            private string? _nome; // uso do caracter _(underline) numa prop/field/atributo/variavel private indica, visualmente, que é uma prop private
            
            definir o elemento publico referente a prp private da classe
            public string? Nome
            {
             esta é a capsula da prop private
                uso dos métodos acessores
                get {return _nome}
                set {_nome = value}
            }
         */
        // definir as propriedades da classe
        // pratica do encapsulamento implicito
        public string? Nome { get; set; }
        public int Idade {  get; set; } 
        public string? Endereco {  get; set; }
        
    }
}
