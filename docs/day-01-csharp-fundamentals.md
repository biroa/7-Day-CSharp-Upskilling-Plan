# Day 1 — C# Language Essentials for Backend Work

Back to the [main upskilling guide](../README.md).

## Daily plan (summary)

- **Focus:** types, nullability, async/await, interfaces, LINQ, exceptions
- **In project:** refactor service/controller for guard clauses and null-safe contracts
- **Deliverable:** cleaner `UserService` and C# cheat-sheet notes

### Timebox (4.5 h)

- 45 min concept deep dive
- 2h 30m guided refactor
- 45 min API verification (Swagger/Postman)
- 30 min notes/recap

## Completion checklist

Use this at the end of Day 1 to confirm you closed the loop.

- [x] **Code refactor done:** `UserService` and `UsersController` updated for null-safe flow, guards, and consistent API responses
- [x] **Validation understood:** can explain DataAnnotations vs Laravel FormRequest in your own words
- [x] **Async basics understood:** can explain why methods return `Task<T>` now and with database I/O later
- [x] **Nullability understood:** can explain `User?`, null checks, and why guard clauses are used
- [x] **LINQ practice done:** used at least `FirstOrDefault`, `Where`, or `Any` intentionally
- [x] **Endpoint verification done:** tested create/read/update/delete and by-email error cases
- [x] **Notes captured:** wrote a short "Laravel -> C#" comparison for today's concepts
- [x] **Retro done (10–15 min):** identified what was easy, what was confusing, and what to revisit on Day 2

## Notes — Validation

In Laravel, request validation is typically handled using FormRequest classes. In ASP.NET Core C#, validation is commonly defined on DTOs using DataAnnotations, where property rules (required, format, length, etc.) are checked against incoming payload values. If validation fails, the API returns validation errors (usually HTTP 400) before business logic executes.

## Notes — Async basics

In ASP.NET Core controllers, `async/await` is used so request-handling threads are not blocked while waiting on I/O operations, improving scalability under concurrent API traffic.

In the service layer, returning `Task<T>` keeps an asynchronous contract even when current operations are in-memory and complete immediately (for example using `Task.FromResult`). In that in-memory case, `async/await` is not required because there is no real asynchronous I/O.

This contract becomes especially useful when moving to a real database (EF Core), where methods naturally await async queries like `ToListAsync()` or `FirstOrDefaultAsync()`.

## Notes — Nullability

`User?` indicates a nullable return type in C#, meaning the method may return either a `User` instance or `null` (for example, when no record is found).

`ArgumentNullException.ThrowIfNull(dto)` is a defensive null check that ensures required reference arguments are not `null` before proceeding, but it does not validate DTO property rules.

DTO property rules (such as required fields or format constraints) are validated through DataAnnotations and model validation.

Guard clauses are early-exit checks used to enforce method preconditions (null, empty, invalid format/range/state) and prevent invalid execution paths.

## Notes — Endpoint verification

Verified with Swagger:

- `POST /api/users` valid payload → `201 Created`
- `POST /api/users` invalid payload → `400 Bad Request`
- `GET /api/users` → `200 OK` (list or empty list)
- `GET /api/users/{id}` missing user → `404 Not Found`
- `GET /api/users/by-email` empty email → `400 Bad Request`
- `GET /api/users/by-email` unknown email → `404 Not Found`
- `PUT /api/users/{id}` existing/missing → `200 OK` / `404 Not Found`
- `DELETE /api/users/{id}` existing/missing → `204 No Content` / `404 Not Found`

## Notes — Laravel to C# comparison

- **Request validation:** Laravel uses `FormRequest` + `$request->validated()`, while C# uses DTO DataAnnotations with `[ApiController]` automatic `400 Bad Request` behavior.
- **Controller responses:** Laravel often returns JSON/resources directly, while C# typically returns `IActionResult`/`ActionResult<T>` with explicit helpers like `Ok`, `CreatedAtAction`, `BadRequest`, and `NotFound`.
- **Service layer pattern:** Laravel services are commonly container-resolved by convention, while C# emphasizes interface-driven services registered in DI (`IUserService` → `UserService` in `Program.cs`).
- **Nullability contracts:** Laravel/PHP nullability is mostly runtime/doc-driven, while C# nullable reference types (such as `User?`) provide compiler-assisted null safety.
- **Guard clauses:** both ecosystems use defensive checks, but in C# they are explicit method precondition checks (`ThrowIfNull`, whitespace/range checks) to fail fast and keep control flow predictable.
- **Async model:** Laravel commonly runs on PHP-FPM (FastCGI Process Manager) process workers, where idle/waiting workers visible in tools like `htop` can still remain tied up per request while waiting on resources; by contrast, C# `async/await` allows threads to be released back to the thread pool during I/O waits, which improves resource efficiency under concurrent API load.
- **Query helpers:** Laravel Collections provide `filter`, `map`, and `firstWhere`; C# uses LINQ equivalents such as `Where`, `Select`, `Any`, and `FirstOrDefault`.
- **In-memory now, DB later:** keeping async contracts in C# service methods now (`Task<T>`) makes migration to EF Core async APIs (`ToListAsync`, `FirstOrDefaultAsync`) straightforward.

## Notes — ActionResult and interfaces

- `ActionResult`/`IActionResult` are designed for HTTP response semantics (status code, payload, headers), not only JSON.
- In ASP.NET Core Web API projects, object results (for example `Ok(object)`, `CreatedAtAction(...)`) are serialized to JSON by default.
- Interfaces such as `IUserService` are not JSON features; they provide abstraction, testability, and dependency injection boundaries.
- JSON shape is primarily controlled by DTOs and serializer configuration, while interfaces keep business logic decoupled from transport details.

## Notes — LINQ practice (common helpers)

Most common helpers you will use in backend services:

- **`Where`** — filter a sequence  
  `var nyUsers = users.Where(u => u.City == "New York");`
- **`Select`** — project/transform values  
  `var emails = users.Select(u => u.Email);`
- **`FirstOrDefault`** — first match or `null` (reference types)  
  `var user = users.FirstOrDefault(u => u.Id == id);`
- **`Any`** — at least one match  
  `var exists = users.Any(u => u.Email == email);`
- **`All`** — all items match a rule  
  `var allAdults = users.All(u => u.BirthDate <= DateTime.UtcNow.AddYears(-18));`
- **`OrderBy` / `OrderByDescending`** — sort  
  `var ordered = users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);`
- **`Count`** — count (optional predicate)  
  `var count = users.Count(u => u.State == "NY");`
- **`Distinct`** — remove duplicates  
  `var cities = users.Select(u => u.City).Distinct();`
- **`Take` / `Skip`** — basic pagination  
  `var page = users.OrderBy(u => u.Id).Skip(10).Take(10);`

**Notes:**

- LINQ queries are composable; you can chain multiple helpers in one expression.
- With EF Core later, prefer async endings such as `ToListAsync()` and `FirstOrDefaultAsync()`.

## Notes — Retro

- **Easy:** service/controller flow and CRUD endpoint structure felt familiar coming from Laravel.
- **Challenging:** separating nullable contracts, guard clauses, and DataAnnotations responsibilities in C#.
- **Key takeaway:** keep async contracts (`Task<T>`) early to make EF Core migration smoother.
- **Revisit on Day 2:** global exception handling and consistent `ProblemDetails` response shape.
