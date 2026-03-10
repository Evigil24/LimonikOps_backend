# CLAUDE.md — LimonikOne Backend

## Project Overview

LimonikOne is a .NET 10 modular monolith backend using Clean Architecture and Domain-Driven Design. PostgreSQL 18 database via EF Core + Npgsql.

## Build & Run

```bash
dotnet restore LimonikOne.slnx
dotnet build LimonikOne.slnx
dotnet run --project src/Host
```

API docs: http://localhost:5100/scalar/v1

## Test Commands

```bash
dotnet test LimonikOne.slnx                        # all tests
dotnet test tests/Architecture.Tests                # architecture rules
dotnet test tests/Modules/Reception.UnitTests       # unit tests
dotnet test tests/Modules/Reception.IntegrationTests # integration (needs Docker)
```

Integration tests use Testcontainers (PostgreSQL) — Docker must be running.

## Formatting

Code is formatted with [CSharpier](https://csharpier.com). Run from repo root:

```bash
dotnet csharpier format .
```

Check formatting without modifying files:

```bash
dotnet csharpier check .
```

## Architecture Rules

- **Modular monolith** with vertical slices per module under `src/Modules/`
- Each module has 4 layers: Domain → Application → Infrastructure → Api
- **Domain** depends on nothing (only `Shared.Abstractions`)
- **Application** depends on Domain
- **Infrastructure** depends on Application + Domain
- **Api** depends on Application + Infrastructure
- Modules must **never** reference each other's internal projects
- These rules are enforced by `tests/Architecture.Tests` using NetArchTest

## Key Patterns

- **No mediator**: controllers inject command/query handlers directly
- **Result<T>** pattern for business failures (no exceptions for expected errors)
- **FluentValidation** validators in Application layer, validated in controllers before handler call
- **Domain events**: in-process only, dispatched after persistence via `IDomainEventDispatcher`
- **UUIDv7** for ID generation
- **IModule** interface for module registration (DI + middleware)

## Code Conventions

- `TreatWarningsAsErrors` is enabled — all warnings must be resolved
- Nullable reference types enabled globally
- Centralized NuGet versions in `Directory.Packages.props`
- Solution file: `LimonikOne.slnx` (modern .slnx format)
- CSharpier for code formatting (local tool in `dotnet-tools.json`)

## Project Structure

```
src/
  Host/                          → Composition root (Program.cs)
  Modules/{Name}/
    {Name}.Domain/               → Entities, value objects, aggregates, domain events
    {Name}.Application/          → Commands, queries, handlers, validators
    {Name}.Infrastructure/       → EF Core DbContext, repositories
    {Name}.Api/                  → Controllers, DTOs, module registration
  Shared/
    Shared.Abstractions/         → ICommand, IQuery, Result, Entity, AggregateRoot, ValueObject
    Shared.Infrastructure/       → Middleware, domain event dispatcher, module loader
tests/
  Architecture.Tests/            → Dependency direction enforcement
  Modules/{Name}.UnitTests/
  Modules/{Name}.IntegrationTests/
```

## Adding a New Module

1. Create 4 projects under `src/Modules/YourModule/` with correct references
2. Implement `IModule` in the Api project for DI registration
3. Reference `YourModule.Api` from Host and add assembly to `Program.cs`
4. Add projects to `LimonikOne.slnx`
5. Add test projects under `tests/Modules/`
