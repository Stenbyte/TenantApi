# TenantAppApi

ASP.NET Core 9 API for laundry booking. Local-first. Postgres via EF Core.

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 17+](https://www.postgresql.org/download/) (Homebrew on macOS is fine)
- Optional: [DBeaver](https://dbeaver.io/download/) to inspect the database

```bash
# macOS (Homebrew) — if Postgres not installed yet
brew install postgresql@17
brew services start postgresql@17
```

Confirm the server is up:

```bash
pg_isready -h localhost -p 5432
```

## Database create settings (first time)

Create an empty database **once**. These settings match the current local `tenantApp` DB and are enough for V1.

| Setting                | Value                      | Why                                                       |
| ---------------------- | -------------------------- | --------------------------------------------------------- |
| Encoding               | `UTF8`                     | Store all text (including Danish letters)                 |
| Collate (`LC_COLLATE`) | `C`                        | Fast, reproducible string sort/compare                    |
| Ctype (`LC_CTYPE`)     | `C`                        | Character classification / case folding                   |
| Text search            | `english` (server default) | Full-text search language; not used for normal `ORDER BY` |

**Collate** = how strings sort. **Ctype** = what counts as a letter/digit and how `LOWER`/`UPPER` behave. **Locale** in UI tools usually sets both together. You generally **cannot** change collate/ctype later without creating a new DB and migrating data.

Danish sort rules (`da_DK.UTF-8`) are **not** required for V1 booking/auth. Revisit only if you need phone-book order for Danish names in lists.

### Create via terminal

```bash
createdb -h localhost -p 5432 -U postgres -E UTF8 -l C -T template0 tenantApp
```

If `createdb` complains about locale, use SQL as the `postgres` user:

```sql
CREATE DATABASE "tenantApp"
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       LC_COLLATE = 'C'
       LC_CTYPE = 'C'
       TEMPLATE = template0;
```

### Verify create settings

```bash
psql -h localhost -p 5432 -U postgres -d tenantApp -c \
"SELECT datname,
        pg_encoding_to_char(encoding) AS encoding,
        datcollate,
        datctype
 FROM pg_database
 WHERE datname = 'tenantApp';"
```

Expect: `UTF8` / `C` / `C`.

## Connection string (appsettings)

`Api/appsettings.json` and `Api/appsettings.Development.json` are **gitignored** (secrets). Create `Api/appsettings.Development.json` locally with at least:

```json
{
  "AllowedOrigins": "http://localhost:5173",
  "Postgres": {
    "Host": "localhost",
    "Port": "5432",
    "DatabaseName": "tenantApp",
    "UserName": "postgres",
    "Password": "YOUR_LOCAL_PASSWORD"
  },
  "JwtSettings": {
    "Secret": "USE_A_LONG_RANDOM_SECRET",
    "Issuer": "TenantApi",
    "Audience": "TenantApiUsers",
    "AccessTokenExpirationMinutes": 5,
    "RefreshTokenExpirationDays": 7
  }
}
```

Names must match: database `tenantApp` ↔ `Postgres:DatabaseName`.

## Migrations (schema)

From repo root:

```bash
dotnet tool install --global dotnet-ef
# or: dotnet tool update --global dotnet-ef

cd Api
dotnet ef database update --project TenantApi.csproj
```

This applies migrations under `Api/Migrations/` (users, buildings, properties, user_properties, refresh token columns, `__EFMigrationsHistory`).

Check applied migrations:

```bash
psql -h localhost -p 5432 -U postgres -d tenantApp -c 'SELECT * FROM "__EFMigrationsHistory";'
```

## Run the API

```bash
cd Api
dotnet run
# or: F5 with launch profile "API: Launch (Development)" → http://localhost:5063
```

Look for a successful Postgres connection log on startup.

## DBeaver (optional)

1. Install: https://dbeaver.io/download/
2. **Database → New Database Connection → PostgreSQL**
3. Host `localhost`, port `5432`, database `tenantApp`, user/password from appsettings
4. Test connection → Finish
5. Browse: `tenantApp` → Schemas → `public` → Tables

Useful SQL in DBeaver:

```sql
SELECT datname, pg_encoding_to_char(encoding), datcollate, datctype
FROM pg_database WHERE datname = 'tenantApp';

SELECT * FROM "__EFMigrationsHistory";

SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY 1;
```

## Web app

Frontend lives in the **TenantWeb** repo. Point `VITE_API_BASE_URL` at `http://localhost:5063` (see TenantWeb `.env`).

## Roadmap

See [ROADMAP.md](./ROADMAP.md).
