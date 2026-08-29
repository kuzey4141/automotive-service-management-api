# Automotive Service Management API

A production-oriented automotive service backend built with ASP.NET Core, Entity Framework Core, and PostgreSQL. The API manages customers, vehicles, service appointments, maintenance history, spare parts, stock movements, and role-protected staff access.

## Features

- Customer and vehicle CRUD with a one-to-many relationship
- Service appointment scheduling and status tracking
- Vehicle maintenance and repair history
- Spare-part catalog, manual stock adjustments, and low-stock reporting
- Transactional part consumption linked to service records
- JWT authentication with `Admin` and `ServiceAdvisor` roles
- One-time initial admin setup and admin-only staff registration
- Standard validation and RFC-style `ProblemDetails` error responses
- EF Core migrations for PostgreSQL
- Unit and HTTP integration tests
- Docker Compose setup for the API and PostgreSQL

## Architecture

The application is a layered modular monolith:

```text
HTTP Request
    -> AutoService.API (controllers, contracts, authentication)
    -> AutoService.Application (use cases and business rules)
    -> AutoService.Infrastructure (EF Core repositories)
    -> PostgreSQL

AutoService.Domain contains the core entities and enums.
```

## Technology Stack

- .NET 10 and ASP.NET Core Web API
- Entity Framework Core 10
- PostgreSQL 18 and Npgsql
- JWT Bearer authentication
- xUnit, NSubstitute, and `WebApplicationFactory`
- Docker and Docker Compose

## Run with Docker

Requirements: Docker Desktop with Docker Compose.

```powershell
Copy-Item .env.example .env
```

Edit `.env` and replace both placeholder secrets, then run:

```powershell
docker compose up --build
```

The API is available at `http://localhost:5080`. Migrations are applied automatically in the container. Check availability with:

```text
GET http://localhost:5080/health
```

## Run Locally

Requirements: .NET 10 SDK and PostgreSQL.

Store secrets outside Git:

```powershell
dotnet user-secrets set "ConnectionStrings:PostgreSql" "Host=localhost;Port=5432;Database=autoservice_db;Username=autoservice_app;Password=YOUR_PASSWORD" --project src/AutoService.API
dotnet user-secrets set "Jwt:Key" "replace-with-a-random-secret-containing-at-least-32-characters" --project src/AutoService.API
```

Apply migrations and start the API:

```powershell
dotnet tool restore
dotnet tool run dotnet-ef database update --project src/AutoService.Infrastructure --startup-project src/AutoService.API
dotnet run --project src/AutoService.API --urls http://localhost:5080
```

## Authentication

Create the first admin once:

```http
POST /api/auth/setup-admin
Content-Type: application/json

{
  "fullName": "System Admin",
  "email": "admin@example.com",
  "password": "ChangeThisPassword123!"
}
```

Then send the returned access token with protected requests:

```http
Authorization: Bearer ACCESS_TOKEN
```

Only an `Admin` can call `POST /api/auth/register` to create additional users. Both roles can use normal service-management endpoints; deleting a spare part is restricted to `Admin`.

## Main Endpoints

| Area | Method and route |
| --- | --- |
| Authentication | `POST /api/auth/setup-admin`, `POST /api/auth/login`, `POST /api/auth/register` |
| Customers | `GET/POST /api/customers`, `GET/PUT/DELETE /api/customers/{id}` |
| Vehicles | `GET/POST /api/vehicles`, `GET/PUT/DELETE /api/vehicles/{id}` |
| Customer vehicles | `GET /api/customers/{customerId}/vehicles` |
| Appointments | `GET/POST /api/appointments`, `GET/PUT/DELETE /api/appointments/{id}` |
| Vehicle appointments | `GET /api/vehicles/{vehicleId}/appointments` |
| Service history | `GET/POST /api/service-records`, `GET/PUT/DELETE /api/service-records/{id}` |
| Vehicle history | `GET /api/vehicles/{vehicleId}/service-records` |
| Spare parts | `GET/POST /api/spare-parts`, `GET/PUT/DELETE /api/spare-parts/{id}` |
| Stock | `POST /api/spare-parts/{id}/stock-adjustments`, `GET /api/spare-parts/low-stock` |
| Used parts | `GET/POST /api/service-records/{recordId}/parts`, `DELETE /api/service-records/{recordId}/parts/{partId}` |

Appointment statuses: `Pending`, `Confirmed`, `InProgress`, `Completed`, `Cancelled`.

Service types: `Maintenance`, `Repair`, `Inspection`, `Other`.

Ready-to-run request examples are available in [`src/AutoService.API/AutoService.API.http`](src/AutoService.API/AutoService.API.http).

In the Development environment, the generated OpenAPI document is available at `/openapi/v1.json`.

## Tests

Run the full test suite:

```powershell
dotnet test AutoService.slnx
```

The unit tests cover authentication, appointment, and inventory business rules. Integration tests run the real HTTP pipeline with an isolated InMemory database and verify JWT protection, customer endpoints, and validation responses.

## Database Model

```text
Customer 1---* Vehicle
Vehicle 1---* ServiceAppointment
Vehicle 1---* ServiceRecord
ServiceAppointment 0---1 ServiceRecord
ServiceRecord *---* SparePart (through ServiceRecordPart)
```

Deleting a vehicle also deletes its appointments and service records. A spare part with usage history cannot be deleted, preserving historical service data.

## Security Notes

- Database passwords and JWT keys are never committed.
- Passwords are stored with ASP.NET Core's password hasher.
- JWT keys must be at least 32 bytes.
- The initial admin endpoint stops working after the first user is created.
- Validation errors do not expose stack traces; unexpected errors return a trace identifier.

## Roadmap

- Reporting dashboard and date-based summaries
- Pagination and filtering for large datasets
- Refresh tokens and account lifecycle management
- CI pipeline with automated test and container build jobs
