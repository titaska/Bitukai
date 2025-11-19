# Versatile POS — .NET Starter

This is a minimal .NET 8 Web API starter to clone/pull and begin coding immediately.

## What's inside
- `src/Pos.Api` — ASP.NET Core minimal API with Swagger and a `/health` endpoint.
- `.github/workflows/dotnet.yml` — GitHub Actions CI to build and run tests (none yet).
- `.gitignore` — Standard Visual Studio/.NET ignores.
- `.editorconfig` — Basic C# code style defaults.
- `old_openapi.yaml` — OpenApi specification of our designed POS system (not to be used).
- `new_openapi.yaml` — OpenApi specification of the POS system which we got from another team (to be used).

## Prerequisites
- .NET SDK 8.x (LTS)
- Optional: Docker (if/when you add containers)

## Getting started
```bash
# restore and run (from repo root)
dotnet restore src/Pos.Api/Pos.Api.csproj
dotnet run --project src/Pos.Api/Pos.Api.csproj
# open http://localhost:5089/swagger
```

## Next steps (suggested)
1. Create separate projects for core services (Orders, Payments, Catalog, Reservations, etc.).
2. Add a shared contracts library (e.g., `src/Pos.Contracts`) for DTOs across services.
3. Stand up a database (e.g., PostgreSQL) and add EF Core migrations.
4. Wire up authentication/authorization and role-based access (Staff/Owner/SuperAdmin).
5. Add CI steps for tests and code quality.

## Setup local DB
1. Open Docker Desktop
2. Run `docker compose up -d` from ...\src directory to start Postgres container
3. In Rider Database window, add a PostgreSQL data source. Use the following settings:
   - Host: localhost
   - Port: 5432
   - User: point_of_sale
   - Password: pass
   - Database: postgres
4. A schema point_of_sale should be seen in the database

## Add database migrations
1. Define an Entity. It should be class in your component model directory after which a table will be created. Use annotations like `[Key], [ForeignKey], [Table], [Column]` and others. Read more about them and their usages.
2. In AppDbContext, add a DbSet for your entity.
3. Run the following commands from ...\src\Pos.Api directory:
   - `dotnet ef migrations add <MigrationName>` - to create a migration (e.g., CreateTaxesTable)
   - `dotnet ef database update` - to apply the migration to the database
4. Check the database to see if the new table is created.
5. If table needs to be altered, change the entity class and repeat step 3.