# LibraryDDD

![CI](https://github.com/CausesHavok/LibraryDDD/actions/workflows/ci.yml/badge.svg)  
[![Line coverage](https://causeshavok.github.io/LibraryDDD/badge_linecoverage.svg)](https://causeshavok.github.io/LibraryDDD/)

A .NET 10 learning project exploring domain modeling, layered architecture, and testing through a library-member registration feature.

This project serves two purposes: a place to practice software design, and a concrete example of how I approach modeling, validation, and application boundaries. It is an evolving prototype, not a production-ready library system.

## Current Scope

The implemented vertical slice is member registration:

- `POST /members/register` accepts registration details.
- The API validates basic request fields and date format, then maps the request to an application command.
- The application assembles a member through domain value objects and invariants.
- A successful registration creates a `MemberRegistered` domain event and returns the generated member ID.
- Persistence currently uses an in-memory repository and unit of work. Data does not survive application restarts.

The domain includes member identity, membership type, contact information, optional address, and staff-ID rules. Borrowing, returns, reservations, and member lifecycle operations are not implemented.

## Structure

| Project | Responsibility |
| --- | --- |
| `LibraryDDD.Domain` | Member aggregate, value objects, domain events, and domain validation |
| `LibraryDDD.Application` | Registration workflow, command assembly, and application errors |
| `LibraryDDD.Infrastructure` | In-memory repository and unit of work |
| `LibraryDDD.API` | Minimal API endpoint, request validation, and API mappings |
| `LibraryDDD.Shared` | Shared result and null-check utilities |

The solution uses a layered design inspired by Clean Architecture and DDD. The goal is to keep domain rules independent of HTTP and persistence concerns.

## Tests

Tests are organized by project and cover domain behavior, application workflows, infrastructure components, shared utilities, and API behavior.

Run the solution tests with:

```bash
dotnet test LibraryDDD.sln
```

API endpoint tests exercise requests through an in-process HTTP host that uses the production registrations (`AddApplication()` and `AddInfrastructure()`). The host assembles them itself instead of running `Program.cs`, so the real startup path is covered by the container smoke test in CI.

## Running with Docker

```bash
docker build -t libraryddd .
docker run --rm -p 8080:8080 libraryddd
```

Register a member:

```bash
curl -X POST http://localhost:8080/members/register \
  -H "Content-Type: application/json" \
  -d '{"name":"Ada Lovelace","email":"ada@example.com","dateOfBirth":"1990-05-17","membershipType":"Adult"}'
```

The response contains the new `memberId`.

## Continuous integration

A GitHub Actions workflow runs on every pull request and on pushes to `main`. It builds the solution, runs the tests with coverage, builds the Docker image, starts the container and sends real requests to it: a valid registration must return `200` with a `memberId`, and an invalid date must return `400`. The first run of this smoke test caught a missing registration for `RegisterMemberHandler` that the in-process tests had hidden.

## Current Limitations

- Persistence is in-memory and intended for development and demonstration only.
- The in-memory repository is a singleton that also holds per-request tracking state, so it is not safe for concurrent requests (see #13).
- There are no workflows for borrowing, returning, or reserving books.
- The project does not claim to be production-ready.

## Design Goals

This project is an ongoing exercise in:

- modeling invariants in domain types
- keeping API, application, domain, and infrastructure responsibilities distinct
- representing validation failures explicitly
- testing behavior at both unit and HTTP boundaries
- making architectural tradeoffs visible and explainable

## Next Steps

1. Separate the shared in-memory store from a per-request repository (#13).
2. Add durable persistence if the project scope calls for it.
3. Expand the domain only when there is a clear feature to model, such as borrowing or reservations.
