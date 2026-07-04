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
├── Services
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
* Service Layer
* Unit of Work
* AutoMapper
* Entity Framework Core

A camada de **Application** é responsável por orquestrar os casos de uso da aplicação, enquanto a camada de **Domain** concentra as regras de negócio e o comportamento das entidades.

Os **Repositories** possuem responsabilidade exclusiva de acesso aos dados, sem conter regras de negócio ou mapeamentos para DTOs.

Os **Services** executam a orquestração dos fluxos da aplicação, validando regras de negócio, utilizando os repositórios e coordenando as operações através do Unit of Work.

---
Embora este projeto utilize Repository Pattern com Services, sua estrutura foi preparada para evoluir facilmente para abordagens mais robustas, como:

* Use Cases (Application Use Cases)
* CQRS
* MediatR
* Domain Driven Design (DDD)
* Event Driven Architecture

Caso o projeto crescesse em complexidade, a migração natural seria substituir os Services por Use Cases específicos, mantendo as mesmas entidades e repositórios.
Posteriormente, seria possível separar completamente comandos e consultas utilizando CQRS, reduzindo o acoplamento entre operações de leitura e escrita.

---

# Regras de negócio implementadas

* Apenas apólices ativas podem abrir novos sinistros.
* Fluxo de status unidirecional:

```
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
* Toda alteração de status gera automaticamente um histórico.

---

# Tecnologias utilizadas

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server (ou SQLite, conforme configuração)
* AutoMapper
* xUnit
* FluentAssertions
* Moq

---

# Bibliotecas utilizadas

| Biblioteca            | Finalidade                       |
| --------------------- | -------------------------------- |
| Entity Framework Core | ORM                              |
| AutoMapper            | Mapeamento entre Entities e DTOs |
| xUnit                 | Testes unitários                 |
| FluentAssertions      | Assertions mais legíveis         |
| Moq                   | Mock de dependências             |

---

# Testes

Foram implementados testes unitários para as principais regras da aplicação, apenas na camada application.

## Camada Application

Testes dos Services:

* criação de sinistro;
* atualização de status;
* criação de apólice;
* consultas;
* validação de exceções;
* verificação das chamadas aos repositórios e Unit of Work.

Por se tratar de um teste técnico, optei por não implementar testes de integração para os Repositories.

---

# Como executar

## Pré-requisitos

* .NET SDK 9
* SQL Server (ou SQLite, conforme configuração)
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

Exemplo para SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SegfyDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Criar banco

Executar as migrations:

```bash
dotnet ef database update --project Segfy.Infrastructure --startup-project Segfy.API
```

Caso ainda não exista nenhuma migration:

```bash
dotnet ef migrations add InitialCreate --project Segfy.Infrastructure --startup-project Segfy.API
```

Depois:

```bash
dotnet ef database update --project Segfy.Infrastructure --startup-project Segfy.API
```

---

## Executar aplicação

```bash
dotnet run --project Segfy.API
```

---

# Endpoints

## Sinistros

### Abrir sinistro

```
POST /api/sinistros
```

---

### Consultar sinistro por Id

```
GET /api/sinistros/{id}
```

---

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

---

### Histórico do sinistro

```
GET /api/sinistros/{id}/historico
```

---

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

### Criar apólice

```
POST /api/apolices
```

---

### Buscar por Id

```
GET /api/apolices/{id}
```

---

### Buscar apólice com sinistros

```
GET /api/apolices/{id}/sinistros
```

---

### Listar apólices

```
GET /api/apolices
```

Filtros:

* status
* data
* page
* pageSize

---

### Atualizar status

```
PATCH /api/apolices/{id}/status
```

# Considerações finais

O objetivo deste projeto foi demonstrar uma arquitetura organizada, desacoplada e de fácil evolução, priorizando boas práticas do ecossistema .NET.

A estrutura atual permite que novas funcionalidades sejam adicionadas com baixo acoplamento e oferece um caminho natural para evolução futura para padrões como Use Cases, CQRS e DDD, sem necessidade de reestruturação significativa da solução.
