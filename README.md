# LibraryDDD

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

API endpoint tests exercise requests through an in-process HTTP host. They currently use test-specific dependency registrations, so they do not yet verify the production dependency-injection composition.

## Current Limitations

- Persistence is in-memory and intended for development and demonstration only.
- The production dependency-injection setup still needs an end-to-end verification pass. In particular, the API resolves `RegisterMemberHandler`, and service registrations and lifetimes must support that request path.
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

1. Verify the production API dependency-injection graph with an end-to-end smoke test.
2. Revisit in-memory service lifetimes and concurrency behavior.
3. Add durable persistence if the project scope calls for it.
4. Expand the domain only when there is a clear feature to model, such as borrowing or reservations.