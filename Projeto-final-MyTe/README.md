# MyTeProject.FrontEnd

## Descrição
Este projeto é uma aplicação ASP.NET Core MVC para gerenciar registros de tempo, despesas e navegação de usuários autenticados. Ele inclui funcionalidades para diferentes papéis de usuário, como Admin, Manager e User.

## Estrutura do Projeto
O projeto está organizado da seguinte forma:

- **Controllers**: Contém os controladores que gerenciam as requisições HTTP e retornam as respostas apropriadas.
- **Models**: Contém as classes de modelo que representam os dados da aplicação.
- **Services**: Contém as classes de serviço que implementam a lógica de negócios e interagem com APIs externas.
- **Utils**: Contém utilitários e enums usados em todo o projeto.

## Controllers

### UserNavigationController
Gerencia a navegação do usuário autenticado, incluindo visualização e postagem de registros de tempo, códigos de cobrança e despesas.

### AdminController
Gerencia a navegação e funcionalidades específicas para usuários com papéis de Admin e Manager.

## Services

### DepartmentService
Implementa a lógica de negócios para gerenciar departamentos, incluindo a obtenção de departamentos com dependências e filtros.

## Models

### UserModel
Representa um usuário no sistema, incluindo informações como nome, email, senha, papel, e outros detalhes.

### ExpenseModel
Representa uma despesa, incluindo informações como data, valor, descrição, e código de cobrança.

### TimeRecordModel
Representa um registro de tempo, incluindo informações como data, tempo apontado, e código de cobrança.

### FortnightModel
Representa um período de quinzena, incluindo uma lista de registros de tempo.

### DepartmentModel
Representa um departamento, incluindo informações como nome, email de contato, e quantidade de funcionários.

## Instalação

1. Clone o repositório:
git clone https://github.com/seu-usuario/MyTeProject.FrontEnd.git

2. Navegue até o diretório do projeto:
cd MyTeProject.FrontEnd

3. Restaure as dependências do projeto: 
dotnet restore

4. Compile o projeto:
dotnet build

5. Execute o projeto:
dotnet run

## Uso

### Navegação de Usuário
- **/UserNavigation/Index**: Redireciona para a visualização de registros de tempo.
- **/UserNavigation/TimeRecord**: Visualiza e posta registros de tempo.
- **/UserNavigation/ChargeCode**: Visualiza e posta códigos de cobrança.
- **/UserNavigation/Expenses**: Visualiza e posta despesas.

### Administração
- **/Admin/Index**: Página inicial para administradores e gerentes.

## Contribuição
1. Faça um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`).
4. Faça o push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licença
Este projeto está licenciado sob a [MIT License](LICENSE).

