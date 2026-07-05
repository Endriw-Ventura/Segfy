# Segfy - API de Gestão de Sinistros

## Sobre o projeto

Este projeto consiste em uma API REST desenvolvida em **.NET 9** para gerenciamento de apólices e sinistros de seguros.

A aplicação foi desenvolvida seguindo os princípios da **Clean Architecture**, separando responsabilidades entre as camadas de API, Application, Domain e Infrastructure, com foco em legibilidade, manutenibilidade e facilidade de evolução.

---

# Arquitetura

O projeto está organizado da seguinte forma:

```text
Segfy.API
│
├── Controllers
├── Middlewares
└── DependencyInjection

Segfy.Application
│
├── DTOs
├── Interfaces
├── Mappings
├── UseCases
└── DependencyInjection

Segfy.Domain
│
├── Entities
├── Enums
├── Exceptions
└── ValueObjects (caso necessário)

Segfy.Infrastructure
│
├── Persistence
│   ├── Context
│   ├── Configurations
│   ├── Repositories
│   └── Migrations
└── DependencyInjection
```

---

# Decisões de Arquitetura

Para este projeto optei por utilizar:

* Clean Architecture
* Repository Pattern
* Use Cases
* Unit of Work
* AutoMapper
* Entity Framework Core

A camada **Application** concentra os casos de uso da aplicação (Use Cases), responsáveis por orquestrar as operações do sistema.

A camada **Domain** contém as entidades, enums e regras de negócio, garantindo que a lógica permaneça independente da infraestrutura.

Os **Repositories** possuem responsabilidade exclusiva de acesso aos dados, sem conter regras de negócio ou mapeamentos para DTOs.

Cada **Use Case** possui responsabilidade única, tornando o código mais organizado, testável e aderente ao princípio **SRP (Single Responsibility Principle)**.

---

# Evolução da arquitetura

Inicialmente, o projeto foi desenvolvido utilizando **Repository Pattern + Services** para simplificar a implementação e manter uma separação clara entre responsabilidades.

Posteriormente, no commit:

> **Feat: implementando UseCases**

foi realizada a migração da camada de Services para uma arquitetura baseada em **Use Cases**, aproximando o projeto de arquiteturas utilizadas em aplicações corporativas.

Essa evolução trouxe benefícios como:

* maior isolamento entre funcionalidades;
* classes menores e com responsabilidade única;
* testes unitários mais simples;
* menor acoplamento entre regras de negócio.

A estrutura atual permite evoluir naturalmente para padrões mais robustos, como:

* CQRS
* MediatR
* Domain Driven Design (DDD)
* Event Driven Architecture

Caso o projeto cresça em complexidade, a migração para CQRS poderá ser realizada praticamente sem alterações na estrutura da solução, substituindo os Use Cases por Commands, Queries e respectivos Handlers.

---

# Regras de negócio implementadas

* Apenas apólices ativas podem abrir novos sinistros.
* Fluxo de status unidirecional:

```text
Aberto
    ↓
Em Análise
    ↓
Aprovado
    ↓
Encerrado

ou

Em Análise
    ↓
Negado
```

* Não é permitido pular etapas do fluxo.
* Ao negar um sinistro, o motivo é obrigatório.
* Ao encerrar um sinistro, o valor aprovado é obrigatório.
* Toda alteração de status gera automaticamente um registro de histórico.
* Número da apólice e número do sinistro possuem restrição de unicidade no banco de dados.

---

# Tecnologias utilizadas

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* AutoMapper
* xUnit
* FluentAssertions
* Moq

---

# Bibliotecas utilizadas

| Biblioteca                                          | Finalidade                            |
| --------------------------------------------------- | ------------------------------------- |
| Entity Framework Core                               | ORM                                   |
| Microsoft.EntityFrameworkCore.SqlServer             | Provedor SQL Server                   |
| Microsoft.EntityFrameworkCore.Tools                 | Migrations                            |
| AutoMapper                                          | Mapeamento entre Entities e DTOs      |
| AutoMapper.Extensions.Microsoft.DependencyInjection | Integração com DI                     |
| xUnit                                               | Framework de testes                   |
| xunit.runner.visualstudio                           | Runner dos testes                     |
| Microsoft.NET.Test.Sdk                              | Infraestrutura de execução dos testes |
| FluentAssertions                                    | Assertions fluentes                   |
| Moq                                                 | Mock de dependências                  |

---

# Testes

Foram implementados testes unitários para a camada **Application**, cobrindo todos os Use Cases da aplicação.

Os testes contemplam:

### Apólices

* Criação de apólice;
* Consulta por Id;
* Consulta com sinistros;
* Consulta paginada;
* Atualização de status.

### Sinistros

* Criação de sinistro;
* Consulta por Id;
* Consulta paginada;
* Consulta do histórico;
* Atualização de status.

Também foram testados cenários de:

* exceções;
* validação das regras de negócio;
* chamadas aos repositórios;
* persistência através do Unit of Work;
* mapeamentos realizados pelo AutoMapper.

Por se tratar de um teste técnico, optei por não implementar testes de integração para os Repositories, focando na validação das regras de negócio da camada de Application.

---

# Como executar

1. Clone o repositório.
2. Execute:

   dotnet run --project Segfy.API

3. A API aplicará automaticamente as migrations em ambiente de desenvolvimento e criará o banco SQLite. (Caso haja algum erro durante este processo, ficará nos logs)

## Pré-requisitos

* .NET SDK 9
* SQL Server
* Entity Framework CLI

Caso não possua a ferramenta do EF instalada:

```bash
dotnet tool install --global dotnet-ef
```

---

## Configurar conexão

Edite a Connection String no arquivo:

```
Segfy.API/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SegfyDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Caso utilize outra instância do SQL Server, basta ajustar a connection string para o seu ambiente.

---

## Criar o banco de dados

Caso ainda não exista uma migration:

```bash
dotnet ef migrations add InitialCreate --project Segfy.Infrastructure --startup-project Segfy.API
```

Aplicar as migrations:

```bash
dotnet ef database update --project Segfy.Infrastructure --startup-project Segfy.API
```

---

## Executar a aplicação

```bash
dotnet run --project Segfy.API
```

A API estará disponível utilizando o endereço configurado pelo ASP.NET Core.

---

# Endpoints

## Sinistros

### Abrir sinistro

```
POST /api/sinistros
```

### Consultar por Id

```
GET /api/sinistros/{id}
```

### Listar sinistros

```
GET /api/sinistros
```

Filtros disponíveis:

| Query    | Descrição             |
| -------- | --------------------- |
| status   | Status do sinistro    |
| data     | Data do sinistro      |
| page     | Página                |
| pageSize | Quantidade por página |

Exemplo:

```
GET /api/sinistros?status=EM_ANALISE&page=1&pageSize=10
```

### Histórico

```
GET /api/sinistros/{id}/historico
```

### Atualizar status

```
PATCH /api/sinistros/{id}/status
```

Body:

```json
{
  "status": "APROVADO",
  "motivoNegativa": null,
  "valorAprovado": 1500
}
```

---

## Apólices

### Criar

```
POST /api/apolices
```

### Buscar por Id

```
GET /api/apolices/{id}
```

### Buscar com sinistros

```
GET /api/apolices/{id}/sinistros
```

### Listar

```
GET /api/apolices
```

Filtros disponíveis:

* status
* data
* page
* pageSize

### Atualizar status

```
PATCH /api/apolices/{id}/status
```

---

# Considerações finais

O objetivo deste projeto foi demonstrar a implementação de uma API utilizando boas práticas do ecossistema .NET, com foco em organização, desacoplamento e facilidade de manutenção.

A adoção de **Use Cases** tornou a camada de Application mais coesa e aderente aos princípios da Clean Architecture. Além disso, a estrutura atual foi preparada para evoluir futuramente para arquiteturas baseadas em **DDD** e **CQRS**, sem necessidade de grandes alterações estruturais, permitindo que o projeto acompanhe naturalmente o aumento de complexidade de uma aplicação real.
