using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentos.CSharp.Polimorfismo
{
    internal class Produto
    {
        // definir as props da classe
        public string? Nome {  get; set; }
        public double Preco {  get; set; }

        // definir um método
        // o método será definido fazendo uso da palavra reservada virtual que possui o seguinte objetivo: proporcionar ao método, posteriormente, ser acessado e possivlemente sobrescrito - a partir do mecanismo de polimorfismo.
        public virtual string ExibirInfos()
        {
            // definir expressão de retorno
            return string.Format("Nome: {0}\nPreço: {1}", Nome.ToUpper(), Preco.ToString());
        }
    }
}
