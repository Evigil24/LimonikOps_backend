# Scale Module — File-by-File Documentation

This document explains each file in the Scale module, what it does, and why it exists. The Scale module handles **weight batch ingestion** from scale devices (e.g., ScalePulse) and **weight event classification** (sessions when items are placed on and removed from the scale).

---

## Architecture Overview

The module follows **Clean Architecture** with four layers:

- **Domain** — Entities, value objects, repository interfaces (no dependencies)
- **Application** — Commands, handlers, validators (depends on Domain)
- **Infrastructure** — EF Core, repositories, database configurations (implements Domain + Application)
- **Api** — Controllers, DTOs, filters, module registration (HTTP entry point)

---

## 1. Scale.Domain

### 1.1 Scale.Domain.csproj

**Purpose:** Project file for the Domain layer.

**Why it exists:** Declares the project, root namespace `LimonikOne.Modules.Scale.Domain`, and references only `Shared.Abstractions`. Domain must have no infrastructure or framework dependencies so it stays pure and testable.

---

### 1.2 WeightBatches/WeightBatchId.cs

**Purpose:** Strongly typed identifier for a weight batch.

**What it does:** A `readonly record struct` wrapping a `Guid`, using UUIDv7 for time-ordered IDs. Provides `New()` (generate new), `From(Guid)` (from existing value), and `ToString()`.

**Why it exists:** Avoids primitive obsession and ensures type safety when passing batch IDs around. UUIDv7 keeps IDs sortable and unique across distributed systems.

---

### 1.3 WeightBatches/WeightBatchEntity.cs

**Purpose:** Aggregate root representing a batch of weight readings sent from a device.

**What it does:** 
- Stores `ExternalBatchId` (from the device), `DeviceId`, `Location`, `SentAt`, `ReceivedAt`
- `Create()` factory method creates a new batch with a new ID and sets `ReceivedAt` to UTC now
- Inherits from `AggregateRoot<WeightBatchId>` (provides `DisplayId` and domain events)

**Why it exists:** A batch is the unit of ingestion. Devices send batches; we persist them and link all readings in that batch. Idempotency is enforced via `ExternalBatchId` in the handler.

---

### 1.4 WeightBatches/IWeightBatchRepository.cs

**Purpose:** Repository interface for weight batches.

**What it does:** 
- `ExistsByExternalBatchIdAsync` — check if a batch was already ingested (for idempotency)
- `AddAsync` — add a new batch

**Why it exists:** Domain defines *what* persistence is needed; Infrastructure implements *how*. This keeps Domain independent of EF Core and database details.

---

### 1.5 WeightReadings/WeightReadingId.cs

**Purpose:** Strongly typed identifier for a single weight reading.

**What it does:** Same pattern as `WeightBatchId` — `readonly record struct` with `New()` and `From(Guid)`.

**Why it exists:** Each reading is an aggregate root with its own identity, needed for tracing measurements back to source readings in weight events.

---

### 1.6 WeightReadings/WeightReading.cs

**Purpose:** Aggregate root representing a single weight measurement within a batch.

**What it does:**
- `BatchId` — links to the parent batch
- `Weight`, `Count`, `FirstTimestamp`, `LastTimestamp`, `StableCount` — measurement data from the device
- `Create()` factory method

**Why it exists:** Readings are the raw data from the scale. They are stored per-batch and then classified into weight events (placing/removing items on the scale).

---

### 1.7 WeightReadings/IWeightReadingRepository.cs

**Purpose:** Repository interface for weight readings.

**What it does:**
- `AddRangeAsync` — add multiple readings in one call
- `GetByBatchIdAsync` — fetch readings for a batch (ordered by `FirstTimestamp`)

**Why it exists:** The ingest handler needs to add readings and, in principle, query by batch. The handler currently adds readings without querying them back; `GetByBatchIdAsync` supports future use cases (e.g., reporting).

---

### 1.8 WeightEvents/WeightEventId.cs

**Purpose:** Strongly typed identifier for a weight event.

**What it does:** Same pattern as other IDs — UUIDv7-backed `readonly record struct`.

**Why it exists:** Each weight event (a “session” on the scale) has a unique identity.

---

### 1.9 WeightEvents/WeightEventStatus.cs

**Purpose:** Enum for the lifecycle of a weight event.

**What it does:** `InProgress` — event is active; `Completed` — event finished (e.g., item removed from scale).

**Why it exists:** At most one open event per device. Status determines whether we add measurements or complete the event.

---

### 1.10 WeightEvents/WeightMeasurementId.cs

**Purpose:** Strongly typed identifier for a measurement within a weight event.

**What it does:** Same pattern as other IDs.

**Why it exists:** Each measurement inside a `WeightEventEntity` is identified so it can be traced to its source reading.

---

### 1.11 WeightEvents/WeightMeasurement.cs

**Purpose:** Value object / entity representing a single measurement within a weight event.

**What it does:** `Weight`, `Timestamp`, `StableCount`, `SourceReadingId` — links back to the `WeightReading` it came from. Internal `Create()` factory.

**Why it exists:** An event aggregates multiple measurements over time (e.g., weight increasing as item is placed). Measurements are owned by the event (EF `OwnsMany`) and provide auditability and traceability to source readings.

---

### 1.12 WeightEvents/WeightEventEntity.cs

**Purpose:** Aggregate root representing a weight event (placing and then removing an item on the scale).

**What it does:**
- `DeviceId`, `Location`, `Status`, `StartedAt`, `EndedAt`, `PeakWeight`
- `Measurements` — collection of `WeightMeasurement`
- `Start()` — creates a new in-progress event with the first measurement
- `AddMeasurement()` — adds a measurement (only when `InProgress`)
- `Complete()` — marks event as completed and sets `EndedAt`

**Why it exists:** Business logic groups readings above a weight threshold into events. An event starts when weight goes above the threshold and ends when it drops below. This models real-world “item on scale” sessions.

---

### 1.13 WeightEvents/IWeightEventRepository.cs

**Purpose:** Repository interface for weight events.

**What it does:**
- `GetOpenEventByDeviceIdAsync` — find the in-progress event for a device
- `AddAsync` — add a new event
- `UpdateAsync` — update an existing event (add measurements, complete)

**Why it exists:** The ingest handler needs to find the open event per device, add measurements, or create a new event. Domain defines this contract; Infrastructure implements it with EF Core.

---

## 2. Scale.Application

### 2.1 Scale.Application.csproj

**Purpose:** Project file for the Application layer.

**What it does:** References Domain, FluentValidation, and declares `InternalsVisibleTo` for Scale.Api and Scale.UnitTests (so the handler can stay internal).

**Why it exists:** Application contains use-case logic, validation, and orchestration. FluentValidation is used for input validation.

---

### 2.2 IScaleUnitOfWork.cs

**Purpose:** Abstraction over “save changes” for the Scale module.

**What it does:** `SaveChangesAsync()` — persists all pending changes.

**Why it exists:** Encapsulates the DbContext `SaveChangesAsync` call. Keeps the handler independent of EF Core and allows a single transactional boundary for batch + readings + events.

---

### 2.3 WeightBatches/Ingest/WeightReadingItem.cs

**Purpose:** DTO for a single reading in the ingest command.

**What it does:** Record with `Weight`, `Count`, `FirstTimestamp`, `LastTimestamp`, `StableCount`.

**Why it exists:** The command carries a list of readings. This shape matches the API payload and is mapped to `WeightReading.Create()` in the handler.

---

### 2.4 WeightBatches/Ingest/IngestWeightBatchCommand.cs

**Purpose:** Command for ingesting a weight batch.

**What it does:** Record implementing `ICommand` with `BatchId`, `DeviceId`, `Location`, `SentAt`, and `Readings` (list of `WeightReadingItem`).

**Why it exists:** Commands represent write operations. This is the single write use case of the Scale module; controllers map HTTP requests to this command.

---

### 2.5 WeightBatches/Ingest/IngestWeightBatchValidator.cs

**Purpose:** FluentValidation validator for `IngestWeightBatchCommand`.

**What it does:**
- `BatchId` required
- `DeviceId` required, max 100 chars
- `Location` required, max 200 chars
- `SentAt` required (not default)
- `Readings` required, max 10,000 items
- Per-reading: `Weight` ≥ 0, `Count` > 0, `StableCount` ≥ 0, `FirstTimestamp` ≤ `LastTimestamp`

**Why it exists:** Validation runs before the handler. Invalid input fails fast with clear error messages, keeping business logic focused on valid data.

---

### 2.6 WeightBatches/Ingest/IngestWeightBatchHandler.cs

**Purpose:** Command handler that ingests a weight batch and classifies readings into events.

**What it does:**
1. **Idempotency:** If `ExistsByExternalBatchIdAsync` returns true, return success (no-op).
2. **Create batch:** `WeightBatchEntity.Create()` and add via repository.
3. **Create readings:** Map each `WeightReadingItem` to `WeightReading.Create()` and add in bulk.
4. **Classify into events:**
   - `WeightThreshold = 0m` — readings above threshold start or extend an event; below threshold completes it
   - Order readings by `FirstTimestamp`
   - If no open event and reading above threshold → start new event
   - If open event and reading above threshold → add measurement
   - If open event and reading below threshold → complete event
5. **Save:** Call `IScaleUnitOfWork.SaveChangesAsync()`.

**Why it exists:** This is the core use case: receive device data, persist it, and derive higher-level “weight events” for reporting or downstream processing.

---

## 3. Scale.Infrastructure

### 3.1 Scale.Infrastructure.csproj

**Purpose:** Project file for the Infrastructure layer.

**What it does:** References Application, Domain, Shared.Infrastructure, and EF Core packages (Npgsql). Declares `InternalsVisibleTo` for Api, IntegrationTests, Schema.Tests.

**Why it exists:** Infrastructure implements persistence. EF Core + Npgsql are implementation details; Application and Domain stay agnostic.

---

### 3.2 Database/ScaleDbContext.cs

**Purpose:** EF Core DbContext for the Scale module.

**What it does:**
- `DbSet<WeightBatchEntity> WeightBatches`
- `DbSet<WeightReading> WeightReadings`
- `DbSet<WeightEventEntity> WeightEvents`
- Uses schema `scale`
- Applies configurations from assembly

**Why it exists:** Defines the database model and schema. All Scale tables live under the `scale` schema to avoid name clashes with other modules.

---

### 3.3 Database/ScaleUnitOfWork.cs

**Purpose:** Implementation of `IScaleUnitOfWork`.

**What it does:** Delegates to `ScaleDbContext.SaveChangesAsync()`.

**Why it exists:** Application needs a save abstraction; Infrastructure provides it using the DbContext.

---

### 3.4 Repositories/WeightBatches/WeightBatchRepository.cs

**Purpose:** Implementation of `IWeightBatchRepository`.

**What it does:**
- `ExistsByExternalBatchIdAsync` — `AnyAsync` on `WeightBatches` by `ExternalBatchId`
- `AddAsync` — `AddAsync` to `WeightBatches` DbSet

**Why it exists:** Concrete persistence for weight batches using EF Core.

---

### 3.5 Repositories/WeightReadings/WeightReadingRepository.cs

**Purpose:** Implementation of `IWeightReadingRepository`.

**What it does:**
- `AddRangeAsync` — add multiple readings to the DbSet
- `GetByBatchIdAsync` — query by `BatchId`, order by `FirstTimestamp`

**Why it exists:** Concrete persistence for weight readings. Used for ingest and for future queries by batch.

---

### 3.6 Repositories/WeightEvents/WeightEventRepository.cs

**Purpose:** Implementation of `IWeightEventRepository`.

**What it does:**
- `GetOpenEventByDeviceIdAsync` — `FirstOrDefaultAsync` with `Include(Measurements)` where `Status == InProgress`
- `AddAsync` — add new event
- `UpdateAsync` — mark entity as modified (for in-memory changes like `AddMeasurement` or `Complete`)

**Why it exists:** Provides access and updates for weight events. Includes `Measurements` so EF can persist owned entities.

---

### 3.7 Database/Configurations/WeightBatches/WeightBatchConfiguration.cs

**Purpose:** EF Core fluent configuration for `WeightBatchEntity`.

**What it does:**
- Table `weight_batches`
- Column mappings (snake_case)
- `Id` as UUID with conversion
- `DisplayId` as identity
- Unique index on `ExternalBatchId`
- Length limits on `DeviceId` (100), `Location` (200)

**Why it exists:** Keeps mapping logic out of the entity and ensures DB schema matches domain expectations. Applied via `ApplyConfigurationsFromAssembly`.

---

### 3.8 Database/Configurations/WeightReadings/WeightReadingConfiguration.cs

**Purpose:** EF Core fluent configuration for `WeightReading`.

**What it does:**
- Table `weight_readings`
- Column mappings
- `BatchId` FK to `weight_batches` with cascade delete
- Index on `BatchId`
- `Weight` precision 18,4

**Why it exists:** Mapping and FK configuration for readings. Cascade delete ensures readings are removed when a batch is deleted.

---

### 3.9 Database/Configurations/WeightEvents/WeightEventConfiguration.cs

**Purpose:** EF Core fluent configuration for `WeightEventEntity`.

**What it does:**
- Table `weight_events`
- Column mappings
- `Status` as string (enum)
- Index on `(DeviceId, Status)` for fast lookup of open events
- `OwnsMany(Measurements)` — `weight_measurements` table with FK `weight_event_id`

**Why it exists:** Configures events and owned measurements. Composite index optimizes `GetOpenEventByDeviceIdAsync`.

---

### 3.10 Database/Migrations (Overview)

Migrations are generated by EF Core and evolve the schema over time. Each migration has a `.cs` (Up/Down logic) and a `.Designer.cs` (model snapshot).

| Migration | Purpose |
|-----------|---------|
| **20260310164402_InitialCreate** | Creates `scale` schema, `weight_batches`, `weight_readings` |
| **20260310175044_ChangeToScaleModule** | Empty (refactor/rename placeholder) |
| **20260313174647_AddWeighingEvents** | Adds `weighing_events` and `weighing_measurements` |
| **20260313182912_SplitWeightReadingAsAggregate** | Adds `display_id` to `weight_readings` |
| **20260313193640_RenameWeighingToWeight** | Renames `weighing_*` tables/columns to `weight_*` |
| **ScaleDbContextModelSnapshot.cs** | Current EF model snapshot for future migrations |

**Why they exist:** EF migrations provide versioned, reversible schema changes. The snapshot is used to generate new migrations.

---

## 4. Scale.Api

### 4.1 Scale.Api.csproj

**Purpose:** Project file for the Api layer.

**What it does:** References Application, Infrastructure, Shared.Abstractions, Shared.Infrastructure, ASP.NET Core, FluentValidation, and health checks.

**Why it exists:** Api is the HTTP entry point. It uses Application and Infrastructure but does not contain business logic.

---

### 4.2 ScaleModule.cs

**Purpose:** Implements `IModule` for Scale module registration.

**What it does:**
- **Config:** Reads `PostgresOptions` for the connection string
- **DbContext:** Registers `ScaleDbContext` with Npgsql
- **Health checks:** Adds `DbContextCheck` for `ScaleDbContext`
- **DI:** Registers `IScaleUnitOfWork`, repositories, `IngestWeightBatchHandler`, `IngestWeightBatchValidator`, `ApiKeyAuthFilter`

**Why it exists:** Host discovers and loads modules via `IModule`. Each module wires its own services and middleware.

---

### 4.3 Controllers/WeightBatches/WeightBatchesController.cs

**Purpose:** HTTP controller for weight batch ingestion.

**What it does:**
- `POST /api/weight-batches` — `Ingest` action
- Uses `ApiKeyAuthFilter` (API key auth when configured)
- Maps `IngestWeightBatchRequest` → `IngestWeightBatchCommand`
- Validates via FluentValidation; returns 400 with validation errors if invalid
- Calls handler; on failure returns 400 with `ProblemDetails`
- On success returns 200 OK

**Why it exists:** Exposes the ingest use case over HTTP. Controllers handle HTTP concerns; validation and business logic stay in Application.

---

### 4.4 Controllers/WeightBatches/Requests/IngestWeightBatchRequest.cs

**Purpose:** DTO for the ingest HTTP request body.

**What it does:** `IngestWeightBatchRequest` with `BatchId`, `DeviceId`, `Location`, `SentAt`, `Readings`. `IngestWeightReadingRequest` with `Weight`, `Count`, `FirstTimestamp`, `LastTimestamp`, `StableCount`.

**Why it exists:** Shapes the API contract. Controllers map from these DTOs to Application commands/DTOs.

---

### 4.5 Filters/ApiKeyAuthFilter.cs

**Purpose:** Optional API key authentication for scale endpoints.

**What it does:**
- Reads configured key from `Scale:ApiKey`
- If not configured → allow all (no auth)
- If configured → require `X-Api-Key` header
- Uses `CryptographicOperations.FixedTimeEquals` to compare keys (timing-safe)

**Why it exists:** Scale devices (e.g., ScalePulse) call the ingest endpoint. API key auth protects it when configured; when not set, development is simpler.

---

## 5. Summary

| Layer | Responsibility |
|-------|----------------|
| **Domain** | Entities, value objects, IDs, repository interfaces — pure business concepts |
| **Application** | Commands, handlers, validation, unit of work — use-case orchestration |
| **Infrastructure** | DbContext, repositories, EF configurations, migrations — persistence |
| **Api** | Controllers, request DTOs, auth filter, module registration — HTTP surface |

The Scale module ingests weight batches from devices, persists batches and readings, and derives weight events (item-on-scale sessions) for downstream use. Each file has a clear role aligned with Clean Architecture and DDD.
