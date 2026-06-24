# MedManage.Modern — Getting Started

Complete guide for running the application locally or with Podman containers.

---

## Prerequisites

| Tool | Version | Purpose |
|------|---------|---------|
| SQL Server | 2016+ | Database (runs on host, not containerized) |
| .NET SDK | 8.0+ | API build & run |
| Node.js | 20.x+ | Angular build & run |
| Podman | 4.x+ | Container runtime (alternative to local dev) |
| podman-compose | latest | Multi-container orchestration |

---

## Database Setup

The MedManage database runs on your host machine (not in a container). Apply the SQL scripts below in order.

### SQL Scripts — Execution Order

Run these against your `MedManage` database in SQL Server Management Studio (SSMS) or via `sqlcmd`:

| # | Script | Purpose | Location |
|---|--------|---------|----------|
| 1 | *(existing schema)* | The MedManage database must already exist with all tables/schemas | Legacy DB backup or EF scaffold |
| 2 | `FixAuditTriggers.sql` | Recreates all 24 audit triggers with explicit column names (replaces broken SELECT * triggers) | `Infrastructure/Scripts/` |
| 3 | `RemoveAuditTriggers.sql` | *(Optional)* Drops old misnamed triggers from prior migrations | `Infrastructure/Scripts/` |


### Applying Scripts

```powershell
# From the project root
sqlcmd -S localhost -d MedManage -i Infrastructure\Scripts\FixAuditTriggers.sql
```

Or open the file in SSMS, connect to your MedManage database, and execute.

### Verify Database Connectivity

```powershell
# Quick TCP check
Test-NetConnection -ComputerName localhost -Port 1433
```

Ensure SQL Server is configured for:
- **TCP/IP enabled** (SQL Server Configuration Manager → Protocols)
- **Mixed mode authentication** (if using SQL login from containers)
- **Firewall allows port 1433** (for Podman container access)

---

## Option A: Run with Podman (Recommended for deployment)

This runs the API, Angular frontend, and jsreport in containers. Only the database stays on the host.

### 1. Create environment file

```powershell
cd MedManage.Modern
copy .env.example .env
```

Edit `.env` with your values:
```env
DB_PASSWORD=YourSqlServerPassword
JWT_SECRET=a-random-string-at-least-32-characters-long
JSREPORT_PASSWORD=admin-password-for-jsreport
```

### 2. Start all containers

```powershell
# Using the convenience script
.\scripts\podman-up.ps1 -Build -Detach

# Or directly with podman-compose
podman-compose up -d --build
```

### 3. Access the application

| Service | URL | Notes |
|---------|-----|-------|
| Frontend (HTTP) | http://localhost:8080 | Angular app (nginx proxies /api to the API) |
| Frontend (HTTPS) | https://localhost:8443 | Same app with self-signed cert (accept browser warning) |
| API | http://localhost:5000 | .NET 8 API (Swagger at /swagger) |
| jsreport | http://localhost:5488 | Report template studio |

### 4. Stop containers

```powershell
.\scripts\podman-down.ps1

# Or remove volumes too
.\scripts\podman-down.ps1 -Volumes
```

### How it works

```
┌─────────────────────────────────────────────────────────┐
│  Podman                                                  │
│                                                          │
│  ┌──────────┐    ┌──────────────┐    ┌───────────────┐  │
│  │  nginx   │───▶│  .NET API    │───▶│  jsreport     │  │
│  │  :80/443 │    │  :8080       │    │  :5488        │  │
│  └──────────┘    └──────┬───────┘    └───────────────┘  │
│  (port 8080/8443)       │                                │
│                         │ host.containers.internal:1433   │
└─────────────────────────┼────────────────────────────────┘
                          │
                          ▼
              ┌──────────────────┐
              │  SQL Server      │  (host machine)
              │  :1433           │
              └──────────────────┘
```

### Troubleshooting Podman

**"Cannot connect to database"**
- Ensure SQL Server allows TCP connections on port 1433
- On Windows with Podman Machine, the host is `host.containers.internal`
- Check your `.env` DB_PASSWORD matches your SQL login

**"Podman machine not running"**
```powershell
podman machine start
```

**"Port already in use"**
```powershell
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

---

## Option B: Run Locally (for development)

Run the API and Angular directly on your machine for hot-reload development.

### 1. Configure the API

Edit `src/MedManage.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

### 2. Start the API

```powershell
cd src/MedManage.API
dotnet run
```

Or with hot-reload:
```powershell
dotnet watch run
```

The API starts at `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).
Swagger UI: `https://localhost:5001/swagger`

### 3. Start the Angular frontend

```powershell
cd client/medmanage-angular
npm install    # first time only
npm start
```

Angular dev server starts at `http://localhost:4200`.

The dev server uses a proxy to forward `/api` requests to the local .NET API:
- **`npm start`** → proxies to `https://localhost:5001` (default, API with HTTPS)
- **`npm run start:http`** → proxies to `http://localhost:5000` (API without HTTPS)

The proxy configs are at `proxy.conf.json` (HTTPS) and `proxy.conf.http.json` (HTTP). Edit the `target` URL if your API runs on a different port.

### 4. Start jsreport (optional, for reports)

```powershell
cd Infrastructure/jsreport
podman-compose up -d
```

Or run it standalone:
```powershell
podman run -d --name jsreport -p 5488:5488 jsreport/jsreport:4.5.0
```

jsreport studio: http://localhost:5488

---

## Running Tests

```powershell
# .NET unit tests
dotnet test

# Angular tests
cd client/medmanage-angular
npm test
```

---

## Project Structure

```
MedManage.Modern/
├── src/
│   ├── MedManage.API/              # ASP.NET Core Web API
│   ├── MedManage.Core/             # Domain entities, interfaces, DTOs
│   ├── MedManage.Infrastructure/   # EF Core, repositories, services
│   ├── MedManage.Identity/         # Authentication services
│   └── MedManage.Shared/           # Shared utilities
├── client/
│   └── medmanage-angular/          # Angular 17 SPA
├── tests/
│   └── MedManage.Infrastructure.Tests/
├── Infrastructure/
│   ├── Scripts/                    # SQL migration scripts
│   └── jsreport/                   # jsreport Docker config + templates
├── scripts/                        # Podman convenience scripts
├── podman-compose.yml              # Full-stack Podman orchestration
├── Dockerfile.api                  # API container build
├── Dockerfile.angular              # Angular container build
├── nginx.conf                      # nginx config for Angular container
├── .env.example                    # Environment variable template
└── docs/                           # Documentation
```

---

## Environment Variables Reference

| Variable | Default | Description |
|----------|---------|-------------|
| `DB_PASSWORD` | *required* | SQL Server password for the connection string |
| `JWT_SECRET` | *required* | JWT signing key (32+ chars) |
| `JSREPORT_PASSWORD` | `password` | jsreport admin password |
| `ASPNETCORE_ENVIRONMENT` | `Production` | .NET environment (Production/Development) |

---

## Useful Commands

```powershell
# Build everything
dotnet build MedManage.Modern.sln

# Run API with hot-reload
cd src/MedManage.API && dotnet watch run

# Build Angular for production
cd client/medmanage-angular && npx ng build --configuration production

# Run Podman stack
podman-compose up -d --build

# View API logs in Podman
podman logs -f medmanage-api

# View all running containers
podman ps

# Rebuild just the API container
podman-compose build api && podman-compose up -d api
```
