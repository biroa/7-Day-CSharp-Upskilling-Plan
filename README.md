# UserApiTest - C# Upskilling Guide

This repository is your practical learning project to transition from PHP/Laravel to C# and ASP.NET Core in a full-stack role.

## Goal

Build job-ready C# backend productivity by evolving this API from in-memory CRUD to a production-style service with persistence, testing, security, and full-stack integration.

## Current Baseline

- ASP.NET Core Web API with dependency injection configured in `Program.cs`
- Controller-based user endpoints in `Controllers/UsersController.cs`
- Service abstraction and implementation in `Services/IUserService.cs` and `Services/UserService.cs`
- DTO validation with DataAnnotations in `Models/CreateUserDto.cs`

## PHP/Laravel -> C#/.NET Mapping

- Service Container -> Built-in DI (`builder.Services.Add...`)
- FormRequest validation -> DataAnnotations + model binding + `[ApiController]` behaviors
- Eloquent model -> POCO entities + EF Core DbContext
- Middleware concept -> Similar request pipeline, order matters in `Program.cs`
- Resource/Transformer -> DTOs + mapping layer

## 7-Day Roadmap (4.5 Hours/Day)

### Day 1 - C# Language Essentials for Backend Work

- Focus: types, nullability, async/await, interfaces, LINQ, exceptions
- In project: refactor service/controller for guard clauses and null-safe contracts
- Deliverable: cleaner `UserService` and C# cheat-sheet notes

Timebox:
- 45 min concept deep dive
- 2h 30m guided refactor
- 45 min API verification (Swagger/Postman)
- 30 min notes/recap

**Day 1 detail:** checklist, tasks, and study notes live in [docs/day-01-csharp-fundamentals.md](docs/day-01-csharp-fundamentals.md).

### Day 2 - ASP.NET Core Request Lifecycle + API Design

- Focus: routing, model binding, action results, status codes, API conventions
- In project: standardize responses + add global exception handling with `ProblemDetails`
- Deliverable: predictable API behavior

Timebox:
- 45 min lifecycle and middleware study
- 2h 15m response/error pipeline improvements
- 60 min negative API test scenarios
- 30 min mapping notes

**Day 2 detail:** practice goals, tasks, and checklist live in [docs/day-02-aspnet-api-design.md](docs/day-02-aspnet-api-design.md).

### Day 3 - Persistence with EF Core

- Focus: DbContext, migrations, tracking vs no-tracking queries (single `User` entity only)
- In project: replace in-memory list with EF Core + Postgres
- Deliverable: migration-based persistent CRUD; **relationship modeling (1:n, n:n, owned types, etc.) is intentionally deferred to a later EF Core deep-dive session once single-entity basics feel solid.**

Timebox:
- 45 min EF Core fundamentals
- 2h 30m DbContext + CRUD migration
- 45 min migration/database verification
- 30 min recap

**Day 3 detail:** practice goals, tasks, and checklist live in [docs/day-03-ef-core-persistence.md](docs/day-03-ef-core-persistence.md).

### Day 4 - Validation, Mapping, Domain Rules, and Basic Relationships

- Focus: DTO separation, constraints, mapping strategy, **first EF Core relationships**, and **idempotent seeding across multiple tables**
- In project: create request/response DTOs, enforce email uniqueness, add a small set of related entities (for example `UserProfile` 1:1 and `Post` 1:n from `User`) with navigation properties and simple `Include` queries, then seed **realistic related rows** in a way that is safe to run repeatedly
- Deliverable: stronger API contract, basic relationship queries working end-to-end, and a **dev-friendly idempotent seed** that populates the related tables without duplicating rows on restart

Timebox:
- 30 min planning
- 2h 15m implementation
- 45 min endpoint + mapping updates
- 30 min idempotent seeding design + migration/verification pass
- 30 min verification/notes

**Laravel ↔ .NET (Day 4 seeding angle):**
- Laravel **`DatabaseSeeder` / `php artisan db:seed`** ↔ a small **hosted seed runner** (Development-only) *or* a **console host** that resolves `AppDbContext` from DI and executes seed logic
- Laravel **“find or create by unique key”** seed patterns ↔ EF idempotency patterns such as **existence checks by natural keys**, **upsert-style updates**, or a **seed marker** row/table to gate reruns
- Prefer wrapping multi-table seeds in a **transaction** so partial inserts do not leave the database half-populated

### Day 5 - Testing Foundations

- Focus: xUnit, integration tests (`WebApplicationFactory`), mocking
- In project: unit tests for service + integration tests for CRUD endpoints
- Deliverable: test suite suitable for CI

Timebox:
- 45 min testing stack overview
- 1h 45m unit tests
- 1h 30m integration tests
- 30 min analysis/gap list

### Day 6 - Security + Production Concerns

- Focus: JWT basics, authorization policies, configuration, logging
- In project: add auth flow and protect user endpoints
- Deliverable: authenticated API baseline

Timebox:
- 45 min auth architecture study
- 2h JWT + policy setup
- 75 min secure endpoint/config work
- 30 min auth verification

### Day 7 - Full-Stack Integration + Team Workflow

- Focus: OpenAPI collaboration, pagination/filtering, Docker/CI basics
- In project: OpenAPI client workflow + API query options + CI/Docker skeletons
- Deliverable: end-to-end workflow baseline

## Daily Routine (270 Minutes)

- 45 min concept study
- 135-150 min implementation
- 30-45 min verification/tests
- 30 min notes ("Laravel vs .NET" mapping)

## Scope Guardrails (Keep 4.5h Sustainable)

- Keep one primary objective per day
- Defer optional enhancements to a parking lot
- Reserve final 30 minutes for validation and notes
- Prefer working software + tests over architecture complexity

## High-Impact Skills for First Weeks

- Read and navigate .NET code quickly (controllers, services, DI, middleware)
- Write robust async code with clear nullability contracts
- Debug with logs and exception pipeline awareness
- Run tests before each commit/PR
- Understand EF Core query behavior and performance basics
- Communicate API contracts with DTOs and OpenAPI

## Week 2+ Extensions

- Command Query Responsibility Segregation (CQRS) + MediatR
- EF Core relationship modeling with multiple entities (1:n, n:n, owned types, delete behaviors)
- Caching (in-memory or Redis)
- Background jobs (HostedService or Hangfire)
- Observability basics (OpenTelemetry)
- Frontend integration patterns (React or Blazor)
