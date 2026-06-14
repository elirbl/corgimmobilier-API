# Ymmo API

Backend REST API for **Corgimmobilier**, a real estate management platform. Built with **ASP.NET Core (.NET 10)**, **Entity Framework Core** and **PostgreSQL**.

## Features

- **Authentication & Authorization** — JWT access/refresh tokens, role-based access control (`Admin`, `Agent`, `Client`)
- **Properties (Biens)** — CRUD, search/filtering (type, status, price, area, city, DPE, agent), pagination and sorting, photo uploads
- **Agencies** — agency management with attached agents and property counts
- **Visits** — scheduling and tracking property visits
- **Transactions** — sales workflow with stage history and document uploads (PDF)
- **Messages** — real-time chat via **SignalR**
- **Favorites** — clients can save/unsave properties
- **Dashboard** — KPIs and analytics (e.g. monthly revenue per agency) using raw SQL with CTEs and window functions
- **Reports** — Excel (EPPlus) and PDF (iText7) export

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core / .NET 10 |
| ORM | Entity Framework Core 10 (Npgsql) |
| Database | PostgreSQL 16 |
| Auth | JWT Bearer + BCrypt password hashing |
| Validation | FluentValidation |
| Real-time | SignalR |
| Docs | Swagger / OpenAPI (Swashbuckle) |
| Exports | EPPlus (Excel), iText7 (PDF) |

## Project Structure

```
Controllers/   API endpoints (Auth, Properties, Agencies, Visits, Transactions, Messages, Favorites, Dashboard, Reports)
Models/        Domain entities (Property, Agency, User, Transaction, Visit, Message, Favorite, ...)
Dtos/          Request/response DTOs
Services/      Repositories and business logic
Validation/    FluentValidation validators
Hubs/          SignalR hubs (chat)
Middleware/    Custom middleware
Data/          EF Core DbContext
Migrations/    EF Core migrations
Database/      Raw SQL scripts (schema, views, seed data)
docs/          Database schema diagrams and documentation assets
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/) and Docker Compose (recommended for running PostgreSQL and the full stack)

### Run with Docker Compose (recommended)

1. Copy the example environment file and fill in your secrets:

   ```bash
   cp .env.docker.example .env.docker
   ```

   ```env
   POSTGRES_USER=ymmo
   POSTGRES_PASSWORD=changeme
   POSTGRES_DB=ymmo_db
   JWT_SECRET=changeme-generate-a-random-base64-secret
   ```

2. Build and start the stack (API, PostgreSQL, and the [frontend](../corgimmobilier-APP)):

   ```bash
   docker compose --env-file .env.docker up -d --build
   ```

   The `--env-file` flag is required — without it, the API will fail to start with `Jwt:Secret n'est pas configuré`.

3. Services will be available at:

   - API: http://localhost:5000 (Swagger UI at `/swagger`)
   - Frontend: http://localhost:3000
   - PostgreSQL: `localhost:5433`

### Run locally (without Docker)

1. Set up a local PostgreSQL instance and update `appsettings.Development.json` with your connection string and JWT configuration.
2. Apply migrations:

   ```bash
   dotnet ef database update
   ```

3. Run the API:

   ```bash
   dotnet run
   ```

## Database

- The full entity-relationship diagram is available at [`docs/schema.png`](docs/schema.png).
- Raw SQL examples (CTE + window functions, views) are illustrated in [`docs/sql-example-1.png`](docs/sql-example-1.png) and [`docs/sql-example-2.png`](docs/sql-example-2.png).

## Contributors

Project made by Elisabeth ROBL and Alexandre RIVIERE