namespace Projeto.ASPNET.MVC.CRUD_02.Models
{
    // ESTA CLASSE STATIC REPRESENTA A ESTRUTURA DE ARMAZENAMENTO DE DADOS DA APLICAÇÃO
    public static class Repository
    {
        // 1º passo: definir todos os elementos que compõem a classe static como elementos static

        // - qual seria o tipo coleção de dados (collection) adequada para este armazenamento simulado - em tempo de execução - da classe Repository? R.: Seria o tipo List<T>
        private static List<Colab> _todosOsColabs = new List<Colab>(); // neste ponto, foi criado o objeto _todosOsColabs para atuar como a lista de armazenamaento - em tempo de execução - de dados seguindo as especificações do model

        // 2º passo: o objeto _todosOsColabs é private - portanto, para acesso "externo" será necessario encapsula-lo. Além disso, será necessario enumerar cada um dos registros. Então, o elemento (encapsulamento) precisa ser definido como "enumeravel"
        public static IEnumerable<Colab> TodosOsColabs
        {
            get { return _todosOsColabs;}

            // definido o encapsulamento como IEnumerable<Colab> basta, agora, fazer referencia ao elemento publico para que todos os registros sejam recuperados e exibidos em tela - na view
        }

        // 3º passo: definir um método para ser acessado e cumprir a tarefa de inserir dados na estrutura simulada. Ao definir o método, será necessario fazer uso do método embarcado/nativo Add() para que sejam, efetivamente, inseridos na estrutura da lista
        public static void Inserir(Colab registroColab)
        {
            // criar a instrução - na integra - de inserção de dados fazendo uso do método Add(). Abaixo, o objeto _todosOsColabs(que nada mais é do que a lista de dados) recebe como valor - para ser adicionada a lista - o argumento oferecido ao parametro do método Inserir(). É dessa forma que o registro será inserido
            _todosOsColabs.Add(registroColab);

        }


        // 4º passo: definir um método que, de forma semelhante a instrução anterior, efetivamente, excluirá um registro - devidamente identificado - da base simulada de armazenamento. Ao definir o método, será necessario fazer uso do método embarcado/nativo Remove() para que, efetivamente, um registro seja excluido da estrutura da lista
        public static void Excluir(Colab registroColab)
        {
            // criar a instrução - na integra - de exclusão de dados fazendo uso do método Remove(). Abaixo, o objeto _todosOsColabs(que nada mais é do que a lista de dados) recebe como valor - para ser excluido da lista - o argumento oferecido ao parametro do método Excluir(). É dessa forma que o registro será excluido
            _todosOsColabs.Remove(registroColab);

        }
    }
}
