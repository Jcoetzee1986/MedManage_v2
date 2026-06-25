# MedManage.Modern

Modern Angular + .NET 8 rewrite of the legacy WinForms MedManage healthcare case management system.

## Quick Start

```powershell
# Option 1: Podman (recommended)
copy .env.example .env        # edit with your DB password
.\scripts\podman-up.ps1 -Build -Detach

# Option 2: Local development
dotnet watch run --project src\MedManage.API  # Terminal 1
cd client\medmanage-angular & npm start       # Terminal 2
```

See [docs/GETTING-STARTED.md](docs/GETTING-STARTED.md) for full setup instructions.

---

## Architecture

| Layer | Technology | Notes |
|-------|-----------|-------|
| Frontend | Angular 17, Material, ag-Grid | Standalone components, lazy-loaded routes |
| API | .NET 8, EF Core 8, AutoMapper | Repository + Unit of Work pattern |
| Auth | JWT + aspnet_Membership | Refresh tokens, session timeout |
| Reports | jsreport 4.5 | PDF/Excel via Docker container |
| Database | SQL Server 2016+ | Shared with legacy (runs on host) |

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   Angular    │────▶│   .NET API   │────▶│  SQL Server  │
│   (nginx)    │     │   (Kestrel)  │     │   (host)     │
└──────────────┘     └──────┬───────┘     └──────────────┘
                            │
                     ┌──────▼───────┐
                     │   jsreport   │
                     └──────────────┘
```

---

## Modules Implemented

| Module | API | Angular | Features |
|--------|-----|---------|----------|
| Authentication | ✅ | ✅ | Login, register, refresh tokens, password reset, session timeout |
| Case Management | ✅ | ✅ | Full CRUD, 15 sub-entity tabs, copy, lock, workflow |
| Finance & Billing | ✅ | ✅ | CRUD, bulk payment, remittance, discounts, summaries |
| Members | ✅ | ✅ | CRUD, chronic illness, products, notes, AllowServices |
| Service Providers | ✅ | ✅ | CRUD, tariffs, custom tariffs, discounts, autocomplete |
| Tariffs | ✅ | ✅ | SP-wrapped lookup/calculation, base/rates/names CRUD |
| Bookings | ✅ | ✅ | CRUD, booking-to-case conversion |
| Medical Aids | ✅ | ✅ | CRUD, products, exclusions, tariff association |
| Episodes | ✅ | ✅ | CRUD, case grouping/linking |
| Code Lookups | ✅ | ✅ | CPT/ICD/NAPPI typeahead search + reusable dialog |
| Reporting | ✅ | ✅ | 4 report types, PDF viewer, Excel export |
| Data Imports | ✅ | ✅ | DRD members, NAPPI codes, billing files |
| Documents | ✅ | ✅ | Upload/download, image thumbnails, gallery |
| Reference Data | ✅ | ✅ | 17 lookup tables with generic CRUD |
| System Admin | ✅ | ✅ | Config, user management, sessions |
| RBAC | ✅ | ✅ | [Authorize(Roles)] + Angular guards + *hasRole directive |
| Business Rules | ✅ | — | Member eligibility, date validation, auth prefix |

---

## Running with Podman

Everything except the database runs in containers. SQL Server stays on the host.

```powershell
# 1. Configure
copy .env.example .env
# Edit .env with your DB_PASSWORD, JWT_SECRET

# 2. Build & start
podman-compose up -d --build

# 3. Access
#    Frontend:  http://localhost:8080
#    API:       http://localhost:5000/swagger
#    jsreport:  http://localhost:5488

# 4. Stop
podman-compose down
```

---

## Running Locally

```powershell
# API (hot-reload)
cd src/MedManage.API
dotnet watch run
# → https://localhost:5001/swagger

# Angular (hot-reload)
cd client/medmanage-angular
npm install   # first time
npm start
# → http://localhost:4200
```

---

## Database Scripts

Apply to your existing MedManage database in this order:

| # | Script | Purpose |
|---|--------|---------|
| 1 | `Infrastructure/Scripts/FixAuditTriggers.sql` | Recreates 24 audit triggers with explicit columns |
| 2 | `Infrastructure/Scripts/RemoveAuditTriggers.sql` | *(Optional)* Cleans up old misnamed triggers |

```powershell
sqlcmd -S localhost -d MedManage -i Infrastructure\Scripts\FixAuditTriggers.sql
```

---

## Project Structure

```
MedManage.Modern/
├── src/
│   ├── MedManage.API/              # 38 controllers, middleware, config
│   ├── MedManage.Core/             # Entities, interfaces, DTOs
│   ├── MedManage.Infrastructure/   # EF Core, 52 repos, 46 services
│   ├── MedManage.Identity/         # Auth service
│   └── MedManage.Shared/           # Shared utilities
├── client/medmanage-angular/       # 12 feature modules, 4 shared components
├── tests/                          # xUnit tests
├── Infrastructure/
│   ├── Scripts/                    # SQL migration scripts
│   └── jsreport/                   # Docker + report templates
├── scripts/                        # podman-up.ps1, podman-down.ps1
├── podman-compose.yml              # Full-stack orchestration
├── Dockerfile.api                  # API container
├── Dockerfile.angular              # Angular + nginx container
├── nginx.conf                      # Reverse proxy config
└── .env.example                    # Environment template
```

---

## Tech Stack

- **Frontend**: Angular 17, TypeScript 5, Angular Material, ag-Grid, RxJS
- **Backend**: .NET 8, ASP.NET Core, EF Core 8, AutoMapper, FluentValidation, SkiaSharp
- **Auth**: JWT Bearer, aspnet_Membership integration
- **Reporting**: jsreport 4.5 (Docker), chrome-pdf + html-to-xlsx recipes
- **Logging**: Serilog → Console + File + Graylog (GELF/UDP)
- **Containers**: Podman + podman-compose

---

## Contributing

1. Branch from `main` using `feature/your-feature-name`
2. Follow existing patterns (controller → service → repository → DTOs)
3. Ensure `dotnet build` and `ng build` pass with 0 errors
4. Submit PR with description of changes
