# LimonikOne — .NET Modular Monolith

A production-ready modular monolith backend built with .NET 10, Clean Architecture, and Domain-Driven Design.

## Architecture

```
src/
  Host/                          → ASP.NET Core Web API (composition root)
  Modules/
    Reception/                   → Example module (full vertical slice)
      Reception.Api/             → Controllers, DTOs, module registration
      Reception.Application/     → Commands, queries, handlers, validators
      Reception.Domain/          → Entities, value objects, aggregates, domain events
      Reception.Infrastructure/  → EF Core DbContext, repository implementations
  Shared/
    Shared.Abstractions/         → Shared contracts (ICommand, IQuery, Result, Entity, etc.)
    Shared.Infrastructure/       → Cross-cutting concerns (middleware, domain event dispatcher)
tests/
  Architecture.Tests/            → Dependency direction and module boundary enforcement
  Modules/
    Reception.UnitTests/         → Domain and application layer unit tests
    Reception.IntegrationTests/  → API integration tests with Testcontainers
```

### Dependency Rules

- **Domain** → depends on nothing (only Shared.Abstractions)
- **Application** → depends on Domain
- **Infrastructure** → depends on Application + Domain
- **Api** → depends on Application (+ Infrastructure for module registration only)
- **Host** → references all modules, acts as composition root

Modules must **never** reference each other's internal projects.

## How to Run

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 18](https://www.postgresql.org/) (or Docker)

### Quick Start

```bash
# Start PostgreSQL (example with Docker)
docker run -d --name limonikone-db \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=limonikone \
  -p 5432:5432 postgres:18

# Restore and run
dotnet restore LimonikOne.slnx
dotnet run --project src/Host

# Open Scalar API docs
# http://localhost:5100/scalar/v1
```

### Configuration

Connection string is in `src/Host/appsettings.json`:

```json
{
  "Postgres": {
    "ConnectionString": "Host=localhost;Port=5432;Database=limonikone;Username=postgres;Password=postgres"
  }
}
```

## Commands and Queries

Each use case is a dedicated command or query type with a corresponding handler. No mediator — controllers inject handlers directly.

### Command Example

```csharp
// 1. Define the command
public sealed record CreateReceptionCommand(
    string FirstName, string LastName, string? Notes) : ICommand<Guid>;

// 2. Create a handler
internal sealed class CreateReceptionHandler : ICommandHandler<CreateReceptionCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        CreateReceptionCommand command, CancellationToken cancellationToken = default)
    {
        // Business logic here
        return Result.Success(newId);
    }
}

// 3. Register in the module's IModule.Register()
services.AddScoped<ICommandHandler<CreateReceptionCommand, Guid>, CreateReceptionHandler>();

// 4. Inject in controller
public async Task<IActionResult> Create(
    [FromServices] ICommandHandler<CreateReceptionCommand, Guid> handler, ...)
```

### Query Example

```csharp
public sealed record GetReceptionByIdQuery(Guid Id) : IQuery<ReceptionDto>;

internal sealed class GetReceptionByIdHandler : IQueryHandler<GetReceptionByIdQuery, ReceptionDto>
{
    public async Task<Result<ReceptionDto>> HandleAsync(
        GetReceptionByIdQuery query, CancellationToken cancellationToken = default)
    {
        // Fetch and return
    }
}
```

## Validation

FluentValidation validators live in the Application layer. Controllers validate explicitly before calling the handler:

```csharp
var validationResult = await validator.ValidateAsync(command, cancellationToken);
if (!validationResult.IsValid)
{
    return ValidationProblem(new ValidationProblemDetails(...));
}

var result = await handler.HandleAsync(command, cancellationToken);
```

Business-rule failures use the `Result` pattern — no exceptions for expected failures. Unexpected errors are caught by global exception middleware and returned as `ProblemDetails`.

## Domain Events

Domain events are in-process only. Aggregates raise events, which are dispatched after persistence succeeds.

```csharp
// 1. Define an event
public sealed record ReceptionCreatedEvent(
    ReceptionId ReceptionId, string GuestFullName) : DomainEvent;

// 2. Raise in aggregate
public static ReceptionEntity Create(GuestName guestName, string? notes)
{
    var reception = new ReceptionEntity { ... };
    reception.RaiseDomainEvent(new ReceptionCreatedEvent(reception.Id, guestName.FullName));
    return reception;
}

// 3. Create a handler in Application layer
internal sealed class ReceptionCreatedEventHandler : IDomainEventHandler<ReceptionCreatedEvent>
{
    public Task HandleAsync(ReceptionCreatedEvent domainEvent, CancellationToken ct = default)
    {
        // React to event
    }
}

// 4. Register the handler
services.AddScoped<IDomainEventHandler<ReceptionCreatedEvent>, ReceptionCreatedEventHandler>();

// 5. Dispatch after saving (in command handler)
await _repository.AddAsync(reception, ct);
await _domainEventDispatcher.DispatchAsync(reception.DomainEvents, ct);
reception.ClearDomainEvents();
```

## How to Add a New Module

1. Create the module folder under `src/Modules/YourModule/` with four projects:
   - `YourModule.Domain` → references `Shared.Abstractions`
   - `YourModule.Application` → references `YourModule.Domain`
   - `YourModule.Infrastructure` → references `YourModule.Application` + `Shared.Infrastructure`
   - `YourModule.Api` → references `YourModule.Application` + `YourModule.Infrastructure`

2. Add `InternalsVisibleTo` from Application and Infrastructure to the Api project.

3. Create a module registration class implementing `IModule`:

   ```csharp
   public sealed class YourModule : IModule
   {
       public string Name => "YourModule";

       public void Register(IServiceCollection services, IConfiguration configuration)
       {
           // Register DbContext, repositories, handlers, validators
       }

       public void Use(IApplicationBuilder app) { }
   }
   ```

4. Reference `YourModule.Api` from the Host project.

5. Add the module assembly to `Program.cs`:

   ```csharp
   var moduleAssemblies = new[]
   {
       typeof(ReceptionModule).Assembly,
       typeof(YourModule).Assembly
   };
   ```

6. Add the projects to `LimonikOne.slnx`.

7. Add test projects under `tests/Modules/`.

## Testing

```bash
# Run all tests
dotnet test LimonikOne.slnx

# Run only unit tests
dotnet test tests/Modules/Reception.UnitTests

# Run architecture tests
dotnet test tests/Architecture.Tests

# Run integration tests (requires Docker for Testcontainers)
dotnet test tests/Modules/Reception.IntegrationTests
```

## Technical Stack

- .NET 10
- PostgreSQL 18 (via Npgsql + EF Core)
- Serilog (structured logging)
- FluentValidation
- Scalar (OpenAPI documentation)
- xUnit + FluentAssertions + NetArchTest + Testcontainers
