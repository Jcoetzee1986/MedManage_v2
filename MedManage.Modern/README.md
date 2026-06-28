# MedManage.Modern

Modern Angular + .NET 10 rewrite of the legacy WinForms MedManage healthcare case management system.

## Quick Start

```powershell
# Option 1: Podman (recommended for deployment)
copy .env.example .env        # edit with your DB password + JWT secret
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
| Frontend | Angular 19, Material, standalone components | Lazy-loaded routes, reactive forms |
| API | .NET 10, EF Core 10, FluentValidation | Repository + Unit of Work pattern |
| Auth | JWT + aspnet_Membership | Refresh tokens, session timeout, RBAC |
| Reports | ClosedXML + PuppeteerSharp | PDF/Excel without external dependencies |
| Letters | Handlebars.NET + PuppeteerSharp | HTML templates → PDF |
| Database | SQL Server 2016+ | Shared with legacy (runs on host) |

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   Angular    │────▶│   .NET API   │────▶│  SQL Server  │
│   (nginx)    │     │   (Kestrel)  │     │   (host)     │
└──────────────┘     └──────────────┘     └──────────────┘
```

---

## Modules Implemented

| Module | API | Angular | Features |
|--------|-----|---------|----------|
| Authentication | ✅ | ✅ | Login, register, refresh tokens, password reset, session timeout |
| Case Management | ✅ | ✅ | Full CRUD, 15 sub-entity tabs, copy, lock, workflow, linked cases |
| Finance & Billing | ✅ | ✅ | CRUD, bulk payment import/export, remittance, discounts |
| Members | ✅ | ✅ | CRUD, chronic illness, products, notes, AllowServices |
| Service Providers | ✅ | ✅ | CRUD, tariffs, custom tariffs, discounts, autocomplete |
| Tariffs | ✅ | ✅ | SP-wrapped lookup/calculation, tariff percentage management |
| Bookings | ✅ | ✅ | CRUD, booking-to-case conversion |
| Medical Aids | ✅ | ✅ | CRUD, products, exclusions, tariff association |
| Episodes | ✅ | ✅ | CRUD, case grouping/linking |
| Code Lookups | ✅ | ✅ | CPT/ICD/NAPPI typeahead search + reusable dialog |
| Reporting | ✅ | ✅ | 7 report types (PDF + Excel), inline PDF viewer |
| Case Letters | ✅ | ✅ | Handlebars templates, per-client customization, discharge/referral forms |
| Data Imports | ✅ | ✅ | DRD members, NAPPI codes, billing CSV import/export |
| Documents | ✅ | ✅ | Upload/download, image thumbnails, gallery |
| Reference Data | ✅ | ✅ | 17 lookup tables with generic CRUD |
| System Admin | ✅ | ✅ | Config, user management, tariff percentage admin |
| RBAC | ✅ | ✅ | [Authorize(Roles)] + Angular guards + *hasRole directive |
| Business Rules | ✅ | — | Member eligibility, date validation, auth prefix |

---

## Running with Podman

Everything except the database runs in containers. SQL Server stays on the host.

```powershell
# 1. Configure
copy .env.example .env
# Edit .env with your DB_PASSWORD and JWT_SECRET

# 2. Build & start
podman-compose up -d --build

# 3. Access
#    Frontend:  http://localhost:8080
#    API:       http://localhost:5000/swagger

# 4. Stop
podman-compose down
```

---

## Running Locally

```powershell
# API (hot-reload)
dotnet watch run --project src\MedManage.API
# → https://localhost:58764/swagger

# Angular (hot-reload)
cd client\medmanage-angular
npm install   # first time
npm start
# → http://localhost:4200
```

---

## Database Scripts

Apply to your existing MedManage database. Scripts are in `Infrastructure/Scripts/` and numbered for execution order.

```powershell
sqlcmd -S . -d MedManage -i Infrastructure\Scripts\001_AddAuditColumns.sql -E
```

---

## Project Structure

```
MedManage.Modern/
├── src/
│   ├── MedManage.API/              # Controllers, middleware, config
│   ├── MedManage.Core/             # Entities, interfaces, DTOs
│   ├── MedManage.Infrastructure/   # EF Core, repositories, services
│   └── MedManage.Shared/           # Shared utilities
├── client/medmanage-angular/       # Angular SPA (standalone components)
├── tests/                          # xUnit tests (61 passing)
├── Infrastructure/
│   ├── Scripts/                    # SQL migration scripts
│   └── Templates/                  # HTML letter templates, base64 images
├── scripts/                        # podman-up.ps1, podman-down.ps1, test scripts
├── podman-compose.yml              # API + Angular orchestration
├── Dockerfile.api                  # API container
├── Dockerfile.angular              # Angular + nginx container
├── nginx.conf                      # Reverse proxy config
└── .env.example                    # Environment template
```

---

## Tech Stack

- **Frontend**: Angular 19, TypeScript 5, Angular Material, RxJS
- **Backend**: .NET 10, ASP.NET Core, EF Core 10, FluentValidation, SkiaSharp
- **Auth**: JWT Bearer with refresh token rotation, aspnet_Membership integration
- **Reports**: ClosedXML (Excel), PuppeteerSharp (PDF), Handlebars.NET (templates)
- **Logging**: Serilog → Console + File + Graylog (GELF/UDP)
- **Containers**: Podman + podman-compose
- **Testing**: xUnit, Moq, FsCheck (property-based tests)

---

## Contributing

1. Branch from `main` using `feature/your-feature-name`
2. Follow existing patterns (controller → service → repository → DTOs)
3. Ensure `dotnet build` and `ng build --configuration production` pass with 0 errors
4. Run tests: `dotnet test` (exclude integration: `--filter "Category!=Integration"`)
5. Submit PR with description of changes
