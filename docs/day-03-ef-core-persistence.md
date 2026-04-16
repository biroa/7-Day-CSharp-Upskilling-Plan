# Day 3 — Persistence with EF Core

Back to the [main upskilling guide](../README.md).

## Daily plan (summary)

- **Focus:** `DbContext`, migrations, tracking vs no-tracking queries
- **In project:** replace in-memory list with EF Core + Postgres
- **Deliverable:** migration-based persistent CRUD

### Timebox (4.5 h)

- 45 min EF Core fundamentals
- 2h 30m `DbContext` + CRUD migration
- 45 min migration/database verification
- 30 min Laravel ↔ .NET mapping notes

## What to practice today

These are the skills and habits that matter most for Day 3.

1. **Modeling with EF Core**
   Define entity configuration clearly (either conventions, attributes, or Fluent API) so your database schema reflects API intent.

2. **`DbContext` lifecycle + DI**
   Understand how `DbContext` is registered in `Program.cs`, scoped per request, and consumed from services without leaking persistence details into controllers.

3. **Migrations as source-controlled schema**
   Treat migrations as part of the app contract. Generate, review, and apply migrations intentionally instead of ad-hoc DB edits.

4. **Async query/write flow**
   Use async EF APIs (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`) consistently to avoid blocking request threads.

5. **Tracking vs no-tracking**
   Use tracking for update/delete flows and `AsNoTracking()` for read-heavy queries when change tracking is not needed.

6. **Repository/service boundaries**
   Keep controller HTTP concerns separate from data access concerns. Services can depend on `DbContext`, but controllers should still stay thin.

## Concepts to study (checklist)

Tick when you can explain each in one paragraph, using *this* project as an example.

- [ ] **`DbContext` basics** — `DbSet<T>`, unit-of-work behavior, change tracking
- [ ] **Provider setup** — Postgres provider/configuration and connection string wiring
- [ ] **Migrations workflow** — add, inspect, apply, rollback strategy
- [ ] **Tracking vs `AsNoTracking()`** — performance + correctness trade-off
- [ ] **Query translation** — LINQ to SQL, what executes server-side
- [ ] **Concurrency and uniqueness basics** — where to enforce constraints (app + DB)

## In-project tasks (suggested order)

Work mainly in `Program.cs`, `Services/UserService.cs`, and new data-layer files you add (for example an `AppDbContext` file and migration files).

1. [ ] **Add EF Core dependencies**
   Install EF Core packages for Postgres and tools (`Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.Design`, and tooling as needed).

2. [ ] **Create `DbContext` + model mapping**
   Add an app `DbContext` with `DbSet<User>` and configure key fields (especially email constraints/index rules if you decide to enforce now or prep for Day 4).

3. [ ] **Register database in `Program.cs`**
   Add connection string config and register `DbContext` in DI (`AddDbContext<...>`).

4. [ ] **Refactor `UserService` off in-memory storage**
   Replace list operations with EF Core queries and `SaveChangesAsync`.

5. [ ] **Generate and apply first migration**
   Create an initial migration and apply it to your Postgres database, then confirm the schema/migrations were applied correctly.

6. [ ] **Run CRUD verification pass**
   Retest all existing user endpoints to confirm behavior parity with persisted data across restarts.

## Migration and verification checklist

- [ ] `dotnet ef migrations add InitialCreate` runs successfully
- [ ] `dotnet ef database update` applies without errors
- [ ] Tables exist in the configured Postgres database (check via psql/admin tool)
- [ ] `GET/POST/PUT/DELETE` still return expected status codes
- [ ] Data persists after API restart
- [ ] Failed queries and exceptions still return controlled error responses

## Manual test ideas

Use Swagger or Postman and verify both status code and data persistence.

| Scenario | Expected result |
|----------|-----------------|
| Create user, then restart app, then get all users | Created user still exists |
| Update existing user, then fetch by id/email | Updated fields are stored |
| Delete user, then fetch again | `404`/not found behavior preserved |
| Create multiple users with distinct emails | All records returned from DB |
| Invalid payload on create/update | Same `400` validation behavior as before |
| Trigger unhandled exception path (if needed) | Global handler still returns controlled `500` |

## Laravel ↔ .NET (Day 3 angle)

- Laravel **Eloquent models + migrations** ↔ EF Core entities + migrations.
- Laravel **`php artisan migrate`** ↔ `dotnet ef database update`.
- Laravel **query builder/Eloquent** ↔ LINQ over `DbSet<T>`.
- Laravel **request lifecycle with DB per request** ↔ scoped `DbContext` per request in ASP.NET Core.
- Laravel **mass assignment concerns** ↔ DTO mapping + explicit model updates in services.

## Completion checklist

Use this at the end of Day 3 to confirm you closed the loop.

- [ ] **Persistence switched:** no in-memory list used by production code path
- [ ] **Migration committed:** schema evolution captured in source
- [ ] **CRUD parity verified:** endpoint behavior still matches Day 2 contracts
- [ ] **Restart proof complete:** data remains after app restart
- [ ] **Tracking decisions documented:** where `AsNoTracking()` is used and why
- [ ] **Notes captured:** short Laravel ↔ .NET persistence mapping
- [ ] **Retro done (10–15 min):** what to carry into Day 4 (DTO/mapping/domain rules)

## Notes — (your space)

_Add your own bullets below as you learn (query behavior surprises, migration gotchas, Postgres quirks, performance notes)._
