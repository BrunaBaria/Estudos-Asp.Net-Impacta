using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentos.CSharp.Sobrecarga.Metodo
{
    internal class EstudoSobrecargaM
    {
        // definir o método contrutor da classe
        public EstudoSobrecargaM() { }

        // definir as descrições do método NomeDeAlguem(){}
        // a tarefa que o método vai cumprir é: receber, como valor, o nome de uma pessoa e exibi-lo - a partir do uso de um objeto. A dinamica de funcionamento do método será baseada na sobrecarga de método
        public string NomeDeAlguem(string PrimeiroNome)
        {
            // expressão de retorno do metodo
            return $"Seu primeiro nome é {PrimeiroNome}";
        }

        // praticar a 1ª sobrecarga do método
        public string NomeDeAlguem(string PrimeiroNome, string NomeDoMeio)
        {
            return $"Seus nomes, o primeiro e o nome do meio são {PrimeiroNome} e {NomeDoMeio}, respectivamente";
        }

        // 2ª sobrecarga do método
        public string NomeDeAlguem(string PrimeiroNome, string NomeDoMeio, string TerceiroNome)
        {
            return $"Seu nome completo é {PrimeiroNome} {NomeDoMeio} {TerceiroNome}";
        }
    }
}
