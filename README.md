# Versatile POS — .NET Starter

This is a minimal .NET 8 Web API starter so your team can clone/pull and begin coding immediately.

## What's inside
- `src/Pos.Api` — ASP.NET Core minimal API with Swagger and a `/health` endpoint.
- `.github/workflows/dotnet.yml` — GitHub Actions CI to build and run tests (none yet).
- `.gitignore` — Standard Visual Studio/.NET ignores.
- `.editorconfig` — Basic C# code style defaults.

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
