# Exchange or Loans API

Este é um projeto de API RESTful desenvolvido com **ASP.NET Core**, **Entity Framework Core**, e **PostgreSQL**. O objetivo do projeto é fornecer um sistema para gerenciamento de usuários em uma plataforma de empréstimos ou trocas de bens.

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework para o desenvolvimento da API.
- **Entity Framework Core**: ORM para facilitar a comunicação com o banco de dados PostgreSQL.
- **PostgreSQL**: Banco de dados utilizado para armazenar os dados de usuários.

## Estrutura do Projeto

- **Controllers**: Contém os controladores da API que expõem os endpoints.
- **Services**: Contém a lógica de negócios da aplicação.
- **Repositories**: Responsável pela interação direta com o banco de dados.
- **Models**: Contém as entidades que representam as tabelas no banco de dados.
- **Migrations**: Contém scripts de migração para criação do banco de dados.

## Instalação
 Requisitos
- **.NET Core 6.0 ou superior**
- **PostgreSQL**

## Passos
Clone o repositorio e instale as dependências
```Bash
    git clone https://github.com/seu-usuario/exchange-or-loans.git
    cd exchange-or-loans
    dotnet restore
```
## Configuração do Banco de Dados:
- Certifique-se de que o PostgreSQL está instalado e rodando.
- Crie um banco de dados para o projeto ou use um já existente.
- Configure a string de conexão no arquivo appsettings.json
```json
{
      "ConnectionStrings":{"DefaultConnection": "Host=localhost;Database=exchange_or_loans;Username=seu_usuario;Password=sua_senha"} 
}

```

## Aplicando migrações
- Para criar o banco de dados e suas tabelas, aplique as migrações:
```bash
{
  dotnet ef database update 
}
```
## Rodando a aplicação
```bash
{
  dotnet run
}
```

### Feito por Matheus Henrique Lourenço Bernardo.