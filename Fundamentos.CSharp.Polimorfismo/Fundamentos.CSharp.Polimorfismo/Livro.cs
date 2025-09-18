using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentos.CSharp.Polimorfismo
{
    // praticar o mecanismo de herança entre as classes Livro e Produto
    internal class Livro : Produto
    {
        // definir uma prop especifica da classe
        public short NPaginas { get; set; }

        // acessar o método ExibirInfos() - que esta definido na classe-pai/superclasse.... - e tentar sobrescreve-lo
        // para colocar em pratica o mecanismo de polimorfismo
        public override string ExibirInfos()
        {
            return base.ExibirInfos() + "\nNumeros de paginas: " + NPaginas.ToString();

                // esta expressão será composta pela referencia ao método descrito na classe-pai e em conjunto com a sobrescrita, aqui, indicada - foi possivel acrescentar algo, em particualr, para que a sobrescrita funcione  de acordo com a necessidade da classe que herda o método - a partir da herança.
        }


    }
}
