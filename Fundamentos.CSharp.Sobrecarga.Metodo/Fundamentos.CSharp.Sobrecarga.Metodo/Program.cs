// praticar a instancia da classe EstudoSobrecargM{}
using Fundamentos.CSharp.Sobrecarga.Metodo;

EstudoSobrecargaM nome = new EstudoSobrecargaM();

// fazer uso do objeto para chamar o método sobrecarregado à suas execuções
Console.WriteLine(nome.NomeDeAlguem("Celio")); // 1ª chamada do método NomeDeAlguem()

Console.WriteLine(new string('-', 50));

Console.WriteLine(nome.NomeDeAlguem("Celio", "Soares")); // 2ª chamada do método sobrecarregado NomeDeAlguem()

Console.WriteLine(new string('-', 50));

Console.WriteLine(nome.NomeDeAlguem("Celio", "Soares", "De Souza"));
