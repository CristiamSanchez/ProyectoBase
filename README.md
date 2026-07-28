# ProyectoBase API

API REST desarrollada con ASP.NET Core 8 siguiendo una arquitectura por capas y buenas prácticas de desarrollo backend.

El proyecto implementa autenticación mediante JWT, autorización basada en roles, acceso a PostgreSQL mediante Entity Framework Core, validaciones, logging, rate limiting, versionado de API, health checks, pruebas unitarias, pruebas de integración y despliegue mediante Docker.

## Tecnologías

- .NET 8
- ASP.NET Core Web API
- C#
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Role-Based Authorization
- AutoMapper
- FluentValidation
- Serilog
- Swagger / OpenAPI
- API Versioning
- Rate Limiting
- xUnit
- Moq
- FluentAssertions
- Docker
- Docker Compose

## Arquitectura

El proyecto está organizado en las siguientes capas:

- Proyecto.Domain
- Proyecto.Application
- Proyecto.Infrastructure
- Proyecto.Api
- Proyecto.Tests
- Proyecto.IntegrationTests

### Domain

Contiene las entidades principales del dominio:

- Cliente
- Producto
- Usuario

### Application

Contiene:

- DTOs
- Interfaces
- Services
- Validators
- AutoMapper Profiles

### Infrastructure

Contiene:

- Entity Framework Core
- PostgreSQL
- DbContext
- Repositories
- Migrations

### API

Contiene:

- Controllers
- Middleware
- Authentication
- Authorization
- Rate Limiting
- API Versioning
- Swagger
- Health Checks
- Logging

## Funcionalidades

### Autenticación

Login mediante JWT:

```http
POST /api/Auth/login