# Template API .NET — Clean Architecture e CQRS

Template de **ASP.NET Core** com **Clean Architecture**, **CQRS** (MediatR), validação com **FluentValidation**, mapeamento com **AutoMapper**, persistência com **Entity Framework Core** e **PostgreSQL**, logging com **Serilog** e documentação com **Swagger**.

---

## Índice

- [Visão geral](#visão-geral)
- [Stack](#stack)
- [Estrutura da solução](#estrutura-da-solução)
- [Pré-requisitos](#pré-requisitos)
- [Primeiros passos](#primeiros-passos)
- [Configuração](#configuração)
- [Autenticação e autorização](#autenticação-e-autorização)
- [Endpoints de referência](#endpoints-de-referência)
- [Padrões e convenções](#padrões-e-convenções)
- [Testes](#testes)
- [Adicionar uma nova entidade](#adicionar-uma-nova-entidade)

---

## Visão geral

O código está organizado em quatro projetos principais:

| Camada | Projeto | Responsabilidade |
|--------|---------|------------------|
| Apresentação | `TemplateApi.API` | Controllers, middleware, Swagger, JWT, CORS |
| Aplicação | `TemplateApi.Application` | Commands, queries, validações, DTOs, MediatR |
| Domínio | `TemplateApi.Domain` | Entidades e contratos do núcleo |
| Infraestrutura | `TemplateApi.Infrastructure` | EF Core, repositórios, serviços (por exemplo JWT) |

**CQRS** separa leitura (queries) de escrita (commands), o que facilita evolução, testes e clareza dos casos de uso.

---

## Stack

| Tecnologia | Uso |
|------------|-----|
| .NET 10 | Runtime e SDK |
| ASP.NET Core | Web API |
| MediatR | Mediator e CQRS |
| FluentValidation | Validação de requests |
| AutoMapper | Mapeamento objeto a objeto |
| EF Core + Npgsql | ORM e PostgreSQL |
| Serilog | Logging estruturado |
| Swashbuckle | OpenAPI / Swagger |
| JWT Bearer | Autenticação stateless |

Versões exatas dos pacotes estão nos arquivos `.csproj` de cada projeto.

---

## Estrutura da solução

```
APINetCore/
├── src/
│   ├── TemplateApi.API/
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   │   └── ExceptionMiddleware.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── TemplateApi.Application/
│   │   ├── Auth/
│   │   ├── Common/          # Behaviors (validação), interfaces
│   │   ├── Products/
│   │   ├── Users/
│   │   ├── Mappings/
│   │   └── DependencyInjection.cs
│   ├── TemplateApi.Domain/
│   │   ├── Entities/
│   │   └── Common/
│   └── TemplateApi.Infrastructure/
│       ├── Persistence/
│       │   ├── Migrations/
│       │   └── Repositories/
│       └── DependencyInjection.cs
└── tests/
    ├── TemplateApi.UnitTests/
    └── TemplateApi.IntegrationTests/
```

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) acessível pela connection string configurada
- IDE opcional: [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio](https://visualstudio.microsoft.com/)

Para comandos `dotnet ef`, a ferramenta global costuma ser necessária:

```bash
dotnet tool install --global dotnet-ef
```

---

## Primeiros passos

### 1. Clonar o repositório

```bash
git clone <url-do-repositório>
cd APINetCore
```

### 2. Configurar o banco

Ajuste `ConnectionStrings:DefaultConnection` em `src/TemplateApi.API/appsettings.json` (ou use variáveis de ambiente / User Secrets em produção).

### 3. Aplicar migrations

```bash
dotnet ef database update \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API
```

O projeto já inclui migrations em `TemplateApi.Infrastructure/Persistence/Migrations/`.

### 4. Executar a API

```bash
dotnet run --project src/TemplateApi.API
```

URLs típicas (conforme `Properties/launchSettings.json`):

- HTTP: `http://localhost:5252`
- HTTPS: `https://localhost:7004`

Swagger UI fica disponível **apenas em ambiente Development**, em `/swagger`.

---

## Configuração

### Connection string (PostgreSQL)

Exemplo no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TemplateAPI;Username=devuser;Password=devpass"
  }
}
```

A infraestrutura registra o `DbContext` com `UseNpgsql` em `TemplateApi.Infrastructure/DependencyInjection.cs`. Para outro provedor (SQL Server, SQLite), troque o pacote NuGet e a chamada `Use*` correspondente nesse arquivo.

### JWT

```json
{
  "Jwt": {
    "Key": "<chave-secreta-com-pelo-menos-32-bytes-utf8>",
    "Issuer": "TemplateApi",
    "Audience": "TemplateApi",
    "ExpiresInMinutes": 60
  }
}
```

A aplicação exige `Jwt:Key`, `Jwt:Issuer` e `Jwt:Audience`. A chave deve ter **pelo menos 32 bytes** em UTF-8 para HS256 (validação em `Program.cs`).

**Importante:** em produção, use segredos fora do repositório (User Secrets, variáveis de ambiente, Azure Key Vault, etc.) e políticas de CORS restritivas. O template usa uma política permissiva (`AllowAll`) apenas para facilitar o desenvolvimento.

### Serilog

O host usa Serilog com leitura da configuração (`ReadFrom.Configuration`). Você pode estender `appsettings.json` com a seção `Serilog` conforme a [documentação do Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration).

---

## Autenticação e autorização

- `POST /api/auth/login` é **anônimo** e retorna um token JWT.
- `ProductsController` e `UsersController` exigem **`[Authorize]`** (Bearer).
- No Swagger (Development), use o botão **Authorize** e informe `Bearer {seu_token}`.

---

## Endpoints de referência

### Autenticação

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/login` | Login (corpo com email e senha) |

### Produtos (autenticado)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/products` | Lista paginada (`page`, `pageSize`) |
| GET | `/api/products/{id}` | Detalhe por ID |
| POST | `/api/products` | Cria |
| PUT | `/api/products/{id}` | Atualiza |
| DELETE | `/api/products/{id}` | Remove |

### Usuários (autenticado)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/users` | Lista |
| GET | `/api/users/{id}` | Detalhe por ID |
| POST | `/api/users` | Cria |
| PUT | `/api/users/{id}` | Atualiza |
| PATCH | `/api/users/{id}/inactivate` | Inativa |

### Exemplo de corpo para criação de produto

```http
POST /api/products
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Notebook",
  "description": "16 GB RAM",
  "price": 4999.90
}
```

### Erros de validação (400)

```json
{
  "errors": {
    "Name": ["Nome é obrigatório."],
    "Price": ["Preço deve ser maior que zero."]
  }
}
```

---

## Padrões e convenções

### Commands e queries

Cada caso de uso costuma ficar em pasta própria, por exemplo:

```
CreateProduct/
├── CreateProductCommand.cs
├── CreateProductCommandHandler.cs
└── CreateProductCommandValidator.cs
```

### Pipeline de validação

O `ValidationBehavior` do MediatR executa os validators do FluentValidation antes do handler. Falhas viram `ValidationException`, tratada pelo middleware.

### Tratamento global de exceções

`ExceptionMiddleware` padroniza respostas:

| Origem | HTTP |
|--------|------|
| `FluentValidation.ValidationException` | 400 Bad Request |
| `KeyNotFoundException` | 404 Not Found |
| `UnauthorizedAccessException` | 401 Unauthorized |
| Demais exceções | 500 Internal Server Error |

### Entidade base

Entidades podem herdar de `BaseEntity` em `TemplateApi.Domain` (`Id`, `CreatedAt`, `UpdatedAt`, método `SetUpdatedAt()`).

---

## Testes

```bash
dotnet test
```

Ou por projeto:

```bash
dotnet test tests/TemplateApi.UnitTests
dotnet test tests/TemplateApi.IntegrationTests
```

---

## Adicionar uma nova entidade

1. **Domain:** nova entidade (por exemplo em `Entities/`) e interfaces necessárias.
2. **Application:** pasta do agregado com `Commands/`, `Queries/`, DTOs, validators e profile do AutoMapper; interface de repositório em `Common/Interfaces` se aplicável.
3. **Infrastructure:** `DbSet` e configuração em `OnModelCreating`, repositório concreto; registro em `DependencyInjection.cs`.
4. **API:** controller com rotas e `[Authorize]` conforme a regra de negócio.
5. **Migrations:**

```bash
dotnet ef migrations add NomeDaMigration \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API

dotnet ef database update \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API
```

---

## Licença

Defina a licença do repositório conforme a política da sua organização (por exemplo, adicione um arquivo `LICENSE` na raiz).
