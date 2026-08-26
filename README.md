# Automotive Service Management API

An automotive service management API built with ASP.NET Core and PostgreSQL.

## Project Status

The project is currently under development. The initial solution and layered project structure have been created.

## Planned Features

- Customer and vehicle management
- Service appointment management
- Maintenance and repair history
- Spare part and inventory tracking
- Authentication and role-based authorization
- Reporting

## Architecture

The solution follows a layered, modular monolith architecture:

- `AutoService.API`: HTTP endpoints and API configuration
- `AutoService.Application`: Use cases and business logic
- `AutoService.Domain`: Core entities and domain rules
- `AutoService.Infrastructure`: Database and external service integrations

## Planned Technology Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT authentication
- xUnit
- Docker

