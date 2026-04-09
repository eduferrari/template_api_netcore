# TemplateApi.NETCore

A template for building ASP.NET Core Web APIs using .NET 10.0, following a multi-layered project structure.

## Overview

This repository provides a foundational structure for a .NET 10.0 Web API. It is designed to be a starting point for developing robust and scalable APIs with a focus on separation of concerns.

### Tech Stack
- **Language:** C# 13 (implied by .NET 10)
- **Framework:** .NET 10.0 (ASP.NET Core Web API)
- **Package Manager:** NuGet
- **API Style:** Minimal APIs
- **Logging:** Serilog
- **Documentation:** Swagger/OpenAPI (Swashbuckle)
- **Authentication:** JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer)

---

## Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later.
- IDE: JetBrains Rider (recommended), Visual Studio 2022, or VS Code with C# Dev Kit.

---

## Project Structure

The project follows a layered architecture (similar to Clean Architecture or Onion Architecture):

- `src/TemplateApi.API`: The entry point for the application. Contains controllers, Minimal API definitions, and application configuration.
- `src/TemplateApi.Application`: Contains business logic, interfaces, and DTOs.
- `src/TemplateApi.Domain`: Contains domain entities and core business logic that is independent of other layers.
- `src/TemplateApi.Infrastructure`: Contains implementations for external services (database, external APIs, etc.).
- `tests/TemplateApi.Tests`: Contains unit and integration tests for the application.

---

## Getting Started

### 1. Clone the repository
```bash
git clone <repository-url>
cd APINetCore
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Run the application
```bash
dotnet run --project src/TemplateApi.API/TemplateApi.API.csproj
```
The API will be available at:
- HTTP: `http://localhost:5252`
- HTTPS: `https://localhost:7004` (if configured)

### 4. Run tests
```bash
dotnet test
```

---

## Scripts

Currently, standard .NET CLI commands are used for common tasks:
- `dotnet build`: Compiles the solution.
- `dotnet test`: Executes tests in the `tests/` directory.
- `dotnet publish`: Prepares the application for deployment.

---

## Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development` by default in `launchSettings.json`. Other typical values include `Staging` and `Production`.
- TODO: Add other custom environment variables as needed for database connections, API keys, etc.

---

## Tests

The project includes a test suite located in the `tests/` directory.
- `TemplateApi.Tests`: Unit tests for the application logic.

Run all tests:
```bash
dotnet test
```

---

## TODOs

- [ ] Configure database connection in `appsettings.json`.
- [ ] Implement actual business logic in `TemplateApi.Application`.
- [ ] Define domain entities in `TemplateApi.Domain`.
- [ ] Implement repository pattern or other infrastructure services in `TemplateApi.Infrastructure`.
- [ ] Add more comprehensive unit and integration tests.

---

## License

- TODO: Add license information (e.g., MIT, Apache 2.0).