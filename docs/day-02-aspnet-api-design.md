# Day 2 — ASP.NET Core Request Lifecycle + API Design

Back to the [main upskilling guide](../README.md).

## Daily plan (summary)

- **Focus:** routing, model binding, action results, status codes, API conventions
- **In project:** standardize responses + add global exception handling with `ProblemDetails`
- **Deliverable:** predictable API behavior across success and failure paths

### Timebox (4.5 h)

- 45 min lifecycle and middleware study
- 2h 15m response/error pipeline improvements
- 60 min negative API test scenarios (Swagger/Postman)
- 30 min Laravel ↔ .NET mapping notes

## What to practice today

These are the skills and habits that matter most for Day 2.

1. **Request pipeline order**  
   Know what runs before your controller: middleware sequence in `Program.cs` (exception handling, HTTPS, routing, endpoints). Understand *where* global error handling should live and why order matters.

2. **Routing and binding**  
   Practice `[Route]`, `[HttpGet]`, `[FromRoute]`, `[FromQuery]`, `[FromBody]`. Be able to explain how a URL maps to an action and how invalid models become `400` with `[ApiController]`.

3. **Consistent HTTP semantics**  
   Same situation → same status code every time: `200`/`201`/`204`/`400`/`404`/`409`/`500` (when you introduce conflict rules later). Avoid mixing plain strings and structured errors on the same API.

4. **`ProblemDetails` (RFC 7807-style errors)**  
   Learn `ProblemDetails`, `ValidationProblemDetails`, and how ASP.NET Core can produce them for validation failures. Goal: clients get a **stable JSON shape** for errors, not only ad-hoc message strings.

5. **Global exception handling**  
   Add middleware or `IExceptionHandler` (depending on .NET version/pattern you choose) so *unexpected* exceptions become a controlled `500` response with `ProblemDetails`, without leaking stack traces in production.

6. **Controller thinness**  
   Keep “what HTTP means” in the controller; keep domain rules in services. Day 2 is a good day to avoid growing controllers with stringly-typed error branches.

## Concepts to study (checklist)

Tick when you can explain each in one paragraph, using *this* project as an example.

- [ ] **Middleware vs endpoint** — what executes in the pipeline before `UsersController`
- [ ] **`[ApiController]`** — automatic `400` for invalid models; binding source inference
- [ ] **`IActionResult` vs `ActionResult<T>`** — when to use which
- [ ] **`ProblemDetails` + `ValidationProblemDetails`** — fields (`type`, `title`, `status`, `detail`, `instance`, `errors`)
- [ ] **Exception middleware / exception handler** — one place for unhandled exceptions
- [ ] **Development vs production** — richer errors in dev, safer responses in prod

## In-project tasks (suggested order)

Work in `Program.cs`, `Controllers/UsersController.cs`, and any new middleware/handler files you add.

1. [ ] **Audit current responses**  
   List each endpoint and document status codes for success and common failures (missing user, bad query, validation error).

2. [ ] **Align error responses with `ProblemDetails`**  
   Replace or supplement raw `BadRequest("...")` / `NotFound("...")` where it makes sense with `Problem(...)` or typed `ProblemDetails` so clients get a consistent JSON shape.

3. [ ] **Add global exception handling**  
   Catch unhandled exceptions once, log them, return `500` + `ProblemDetails` (no sensitive details in production).

4. [ ] **Confirm validation path**  
   Trigger a bad `POST` body and confirm you still get a clear `400` with validation details (built-in behavior + your consistency goals).

5. [ ] **Negative testing pass**  
   Run the scenarios in [Negative test ideas](#negative-test-ideas) below; note any inconsistent bodies or status codes and fix them.

## Negative test ideas

Use Swagger or Postman and record expected `status` + body shape.

| Scenario | Hint |
|----------|------|
| Invalid JSON or missing required fields on `POST /api/users` | Expect `400` + validation problem shape |
| `GET /api/users/{id}` for missing id | Expect `404` |
| `GET /api/users/by-email` with empty/missing `email` | Expect `400` |
| `GET /api/users/by-email` with unknown email | Expect `404` |
| `PUT` / `DELETE` for missing user | Expect `404` |
| Force an unhandled exception in dev (temporary throw) | Expect `500` + controlled `ProblemDetails`, not raw exception page in API |

## Laravel ↔ .NET (Day 2 angle)

- Laravel **middleware** (`app/Http/Kernel.php` stack) ↔ ASP.NET Core **middleware** in `Program.cs` (order matters similarly).
- Laravel **`Handler` / `render` / `report`** for exceptions ↔ global exception handler or exception middleware + logging in .NET.
- Laravel **`$request->validate()` errors** as JSON ↔ model validation + `ValidationProblemDetails` in .NET.
- Returning **`response()->json([...], 422)`** ↔ returning `Problem()` / `ValidationProblem` with explicit status codes in .NET.

## Completion checklist

Use this at the end of Day 2 to confirm you closed the loop.

- [ ] **Pipeline understood:** can sketch middleware order and where exceptions are handled
- [ ] **Responses standardized:** success and error paths use predictable status codes and JSON shapes
- [ ] **`ProblemDetails` in use:** at least validation and one error path use structured errors
- [ ] **Global handler added:** unhandled exceptions become safe `500` responses
- [ ] **Negative tests run:** documented or checked off against the table above
- [ ] **Notes captured:** short Laravel ↔ .NET comparison for middleware and errors
- [ ] **Retro done (10–15 min):** what to carry into Day 3 (EF Core)

## Notes — (your space)

_Add your own bullets here as you learn (middleware order, serializer quirks, anything that surprised you)._
