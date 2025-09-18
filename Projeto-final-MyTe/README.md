# MyTeProject.FrontEnd

## Descri��o
Este projeto � uma aplica��o ASP.NET Core MVC para gerenciar registros de tempo, despesas e navega��o de usu�rios autenticados. Ele inclui funcionalidades para diferentes pap�is de usu�rio, como Admin, Manager e User.

## Estrutura do Projeto
O projeto est� organizado da seguinte forma:

- **Controllers**: Cont�m os controladores que gerenciam as requisi��es HTTP e retornam as respostas apropriadas.
- **Models**: Cont�m as classes de modelo que representam os dados da aplica��o.
- **Services**: Cont�m as classes de servi�o que implementam a l�gica de neg�cios e interagem com APIs externas.
- **Utils**: Cont�m utilit�rios e enums usados em todo o projeto.

## Controllers

### UserNavigationController
Gerencia a navega��o do usu�rio autenticado, incluindo visualiza��o e postagem de registros de tempo, c�digos de cobran�a e despesas.

### AdminController
Gerencia a navega��o e funcionalidades espec�ficas para usu�rios com pap�is de Admin e Manager.

## Services

### DepartmentService
Implementa a l�gica de neg�cios para gerenciar departamentos, incluindo a obten��o de departamentos com depend�ncias e filtros.

## Models

### UserModel
Representa um usu�rio no sistema, incluindo informa��es como nome, email, senha, papel, e outros detalhes.

### ExpenseModel
Representa uma despesa, incluindo informa��es como data, valor, descri��o, e c�digo de cobran�a.

### TimeRecordModel
Representa um registro de tempo, incluindo informa��es como data, tempo apontado, e c�digo de cobran�a.

### FortnightModel
Representa um per�odo de quinzena, incluindo uma lista de registros de tempo.

### DepartmentModel
Representa um departamento, incluindo informa��es como nome, email de contato, e quantidade de funcion�rios.

## Instala��o

1. Clone o reposit�rio:
git clone https://github.com/seu-usuario/MyTeProject.FrontEnd.git

2. Navegue at� o diret�rio do projeto:
cd MyTeProject.FrontEnd

3. Restaure as depend�ncias do projeto: 
dotnet restore

4. Compile o projeto:
dotnet build

5. Execute o projeto:
dotnet run

## Uso

### Navega��o de Usu�rio
- **/UserNavigation/Index**: Redireciona para a visualiza��o de registros de tempo.
- **/UserNavigation/TimeRecord**: Visualiza e posta registros de tempo.
- **/UserNavigation/ChargeCode**: Visualiza e posta c�digos de cobran�a.
- **/UserNavigation/Expenses**: Visualiza e posta despesas.

### Administra��o
- **/Admin/Index**: P�gina inicial para administradores e gerentes.

## Contribui��o
1. Fa�a um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudan�as (`git commit -m 'Adiciona nova feature'`).
4. Fa�a o push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licen�a
Este projeto est� licenciado sob a [MIT License](LICENSE).

