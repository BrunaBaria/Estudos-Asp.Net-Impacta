/*PRATICAS DO POLIMORFISMO*/

// indicação da diretiva
using Fundamentos.CSharp.Polimorfismo;

// praticar a instancia da classe Produto{}
Produto produto = new Produto();

// fazer uso do objeto
produto.Nome = "Sabão em pó";
produto.Preco = 19.99;

// exibir as informações a partir do objeto produto
Console.WriteLine("Informações sobre o produto: " + produto.ExibirInfos());

Console.WriteLine(new string('-', 50));

// praticar a instancia da classe Livro{}
Livro hq = new Livro();

// fazer uso do objeto
hq.Nome = "A morte do Superman";
hq.Preco = 63.85;
hq.NPaginas = 90;

// fazer a chamada do método ExibirInfos() - agora, sobrescrito
Console.WriteLine("Informações sobre o Livro: " + hq.ExibirInfos());
