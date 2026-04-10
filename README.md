# 🚀 Template API .NET com CQRS

Template de projeto ASP.NET Core seguindo **Clean Architecture** e o padrão **CQRS** com MediatR. Pronto para uso em produção, com validação, mapeamento, logging e tratamento global de erros.

---

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Como Usar](#como-usar)
- [Configuração](#configuração)
- [Padrões e Convenções](#padrões-e-convenções)
- [Endpoints](#endpoints)
- [Como Adicionar uma Nova Entidade](#como-adicionar-uma-nova-entidade)

---

## Visão Geral

Este template implementa uma API RESTful com separação clara de responsabilidades em quatro camadas:

- **Domain** — entidades, regras de negócio e contratos
- **Application** — casos de uso com CQRS (Commands e Queries), DTOs, validações e mapeamentos
- **Infrastructure** — persistência com Entity Framework Core e implementação dos repositórios
- **API** — controllers, middleware e configuração da aplicação

O padrão CQRS separa operações de leitura (Queries) das operações de escrita (Commands), tornando o código mais organizado, testável e escalável.

---

## Tecnologias

| Pacote | Versão | Finalidade |
|---|---|---|
| .NET | 8 / 9 | Plataforma |
| ASP.NET Core | 8+ | Web API |
| MediatR | 12+ | CQRS e Mediator |
| FluentValidation | 11+ | Validação de comandos |
| AutoMapper | 13+ | Mapeamento de objetos |
| Entity Framework Core | 8+ | ORM / Persistência |
| Serilog | 8+ | Logging estruturado |
| Swashbuckle (Swagger) | 6+ | Documentação da API |

---

## Estrutura do Projeto

```
📁 MyApi/
├── 📁 src/
│   ├── 📁 TemplateApi.API/                         # Camada de apresentação
│   │   ├── Controllers/
│   │   │   └── ProductsController.cs
│   │   ├── Middleware/
│   │   │   └── ExceptionMiddleware.cs
│   │   └── Program.cs
│   │
│   ├── 📁 TemplateApi.Application/                 # Casos de uso
│   │   ├── Common/
│   │   │   ├── Behaviors/
│   │   │   │   └── ValidationBehavior.cs     # Pipeline de validação
│   │   │   └── Interfaces/
│   │   │       └── IProductRepository.cs
│   │   ├── Products/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateProduct/
│   │   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   │   └── CreateProductCommandValidator.cs
│   │   │   │   ├── UpdateProduct/
│   │   │   │   └── DeleteProduct/
│   │   │   ├── Queries/
│   │   │   │   ├── GetProductById/
│   │   │   │   └── GetAllProducts/
│   │   │   └── DTOs/
│   │   │       └── ProductDto.cs
│   │   ├── Mappings/
│   │   │   └── ProductMappingProfile.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── 📁 TemplateApi.Domain/                      # Núcleo do negócio
│   │   ├── Entities/
│   │   │   └── Product.cs
│   │   └── Common/
│   │       └── BaseEntity.cs
│   │
│   └── 📁 TemplateApi.Infrastructure/              # Infraestrutura
│       ├── Persistence/
│       │   ├── AppDbContext.cs
│       │   └── Repositories/
│       │       └── ProductRepository.cs
│       └── DependencyInjection.cs
│
└── 📁 tests/
    └── TemplateApi.Tests/
```

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download) ou superior
- [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- SQL Server, PostgreSQL ou SQLite (conforme configuração)

---

## Como Usar

### 1. Clonar ou copiar o template

```bash
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### 2. Configurar a connection string

Edite o arquivo `src/TemplateApi.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyApiDb;Trusted_Connection=true"
  }
}
```

### 3. Aplicar as migrations

```bash
dotnet ef migrations add InitialCreate \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API

dotnet ef database update \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API
```

### 4. Executar a aplicação

```bash
dotnet run --project src/TemplateApi.API
```

Acesse a documentação Swagger em: `https://localhost:5001/swagger`

---

## Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Jwt": {
    "Key": "sua-chave-secreta-com-pelo-menos-32-chars",
    "Issuer": "TemplateApi",
    "Audience": "TemplateApi",
    "ExpiresInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Banco de dados suportados

Altere o pacote e a chamada no `DependencyInjection.cs` da Infrastructure:

| Banco | Pacote NuGet | Método EF Core |
|---|---|---|
| SQL Server | `Microsoft.EntityFrameworkCore.SqlServer` | `UseSqlServer(...)` |
| PostgreSQL | `Npgsql.EntityFrameworkCore.PostgreSQL` | `UseNpgsql(...)` |
| SQLite | `Microsoft.EntityFrameworkCore.Sqlite` | `UseSqlite(...)` |

---

## Padrões e Convenções

### Commands e Queries

Cada operação fica em sua própria pasta com os arquivos correspondentes:

```
CreateProduct/
├── CreateProductCommand.cs          # Record com os dados de entrada
├── CreateProductCommandHandler.cs   # Lógica do caso de uso
└── CreateProductCommandValidator.cs # Regras de validação (FluentValidation)
```

### Validação via Pipeline

O `ValidationBehavior<TRequest, TResponse>` intercepta automaticamente todas as requisições ao MediatR e executa os validators registrados antes de chegar ao handler. Erros de validação são convertidos em resposta `400 Bad Request` pelo `ExceptionMiddleware`.

### Tratamento de Erros

O `ExceptionMiddleware` centraliza o tratamento de exceções e retorna respostas padronizadas:

| Exceção | Status HTTP |
|---|---|
| `FluentValidation.ValidationException` | 400 Bad Request |
| `KeyNotFoundException` | 404 Not Found |
| `UnauthorizedAccessException` | 401 Unauthorized |
| Qualquer outra | 500 Internal Server Error |

### Entidade Base

Todas as entidades herdam de `BaseEntity`:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
}
```

---

## Endpoints

Exemplo com a entidade `Product`:

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/products` | Lista todos (paginado) |
| `GET` | `/api/products/{id}` | Busca por ID |
| `POST` | `/api/products` | Cria novo |
| `PUT` | `/api/products/{id}` | Atualiza |
| `DELETE` | `/api/products/{id}` | Remove |

### Exemplo de requisição

```http
POST /api/products
Content-Type: application/json

{
  "name": "Notebook",
  "description": "Notebook 16GB RAM",
  "price": 4999.90
}
```

### Exemplo de resposta de erro de validação

```json
{
  "errors": {
    "Name": ["Nome é obrigatório."],
    "Price": ["Preço deve ser maior que zero."]
  }
}
```

---

## Como Adicionar uma Nova Entidade

Siga os passos abaixo para adicionar, por exemplo, uma entidade `Category`:

**1. Domain** — crie `src/TemplateApi.Domain/Entities/Category.cs` herdando de `BaseEntity`.

**2. Application** — crie a pasta `src/TemplateApi.Application/Categories/` com as subpastas `Commands/`, `Queries/` e `DTOs/`. Adicione a interface `ICategoryRepository` em `Common/Interfaces/`.

**3. Infrastructure** — adicione `DbSet<Category>` no `AppDbContext`, configure o mapeamento no `OnModelCreating` e implemente `CategoryRepository`.

**4. API** — crie `CategoriesController.cs` injetando `IMediator` e mapeando os endpoints para os Commands e Queries correspondentes.

**5. Migration** — gere e aplique a migration:

```bash
dotnet ef migrations add AddCategory \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API

dotnet ef database update \
  --project src/TemplateApi.Infrastructure \
  --startup-project src/TemplateApi.API
```

---

## Licença

Este projeto está sob a licença MIT. Consulte o arquivo [LICENSE](LICENSE) para mais detalhes.