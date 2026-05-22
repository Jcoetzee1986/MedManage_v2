# MedManage Modernization Project

## 🎯 Project Overview

Migration of legacy WinForms MedManage application to modern Angular + .NET Core architecture.

**Legacy System:**
- 84 database tables across 8 schemas
- 278+ stored procedures
- 58 WinForms screens
- Enterprise Library 5.0 data access
- SQL Server Reporting Services (SSRS)
- Multi-tenant healthcare case management system

**Target System:**
- Angular 21 front-end (TypeScript, NgRx, Angular Material)
- .NET 10 Web API backend
- Entity Framework Core 10
- JWT authentication using existing aspnet_Membership tables
- jsreport (open source reporting)
- On-premises IIS deployment

## 📊 Migration Strategy

**Approach:** Parallel migration - build new system alongside legacy for gradual transition

**Key Decisions:**
- Shared database with MainClientID tenant isolation
- Convert simple CRUD SPs to EF Core, keep complex SPs callable via FromSqlRaw
- Clean architecture: API, Core, Infrastructure, Identity, Shared projects
- Repository pattern with Unit of Work for data access

## 🏗️ Solution Structure

```
MedManage.Modern/
├── src/
│   ├── MedManage.API/              # ASP.NET Core Web API
│   ├── MedManage.Core/             # Domain models, interfaces, business logic
│   ├── MedManage.Infrastructure/   # EF Core, repositories, data access
│   ├── MedManage.Identity/         # (Reserved - not currently used)
│   └── MedManage.Shared/           # DTOs, constants, common utilities
├── client/
│   └── medmanage-angular/          # Angular 17+ workspace
├── tests/
│   ├── MedManage.API.Tests/
│   ├── MedManage.Core.Tests/
│   └── MedManage.Infrastructure.Tests/
├── docs/
│   ├── api-design.md
│   ├── stored-procedures-analysis.md
│   ├── database-schema.md
│   └── deployment-guide.md
├── scripts/
│   ├── ef-migrations/
│   ├── data-validation/
│   └── deployment/
└── MedManage.Modern.sln
```

## 📅 Implementation Plan (24 Weeks)

### Phase 1: Foundation Setup (Weeks 1-2) ✅ CURRENT PHASE
- [x] Create solution structure in `/MedManage.Modern/`
- [x] Initialize .NET 8 solution with all projects
- [x] Initialize Angular 17 workspace
- [ ] Scaffold EF Core models from Tables.sql (84 tables)
- [ ] Configure connection strings for shared database
- [ ] Set up repository pattern and base infrastructure
- [ ] Configure Git branching strategy

**Deliverable:** Working skeleton with build pipeline ✅

---

### Phase 2: Authentication & Infrastructure (Weeks 2-3) ✅ COMPLETE
- [x] JWT authentication with existing aspnet_Membership tables
- [x] Build authentication API endpoints (login, register, refresh, logout, password reset)
- [x] Implement Angular authentication service with HTTP interceptors
- [x] Create auth guards and role-based access control
- [x] Configure CORS policy for Angular-API communication
- [x] Refresh token system with rotation and revocation
- [x] Session timeout with activity tracking (configurable, 30-min default)
- [x] Password reset flow with email PIN verification
- [x] Complete frontend auth components (login, register, forgot-password, reset-password)
- [x] Automatic token refresh on 401 errors
- [x] Global design system with Material UI

**Design Decision:** Using existing aspnet_Membership tables (no migration to ASP.NET Core Identity required)

**Deliverable:** Working authentication flow ✅

---

### Phase 3: Core Data Access Layer (Weeks 3-5) 🎯 CURRENT PHASE
- [x] Analyze stored procedures from `Views and stored procedures.sql`
- [x] Create SP conversion priority matrix (278+ procedures)
- [x] Implement generic repository pattern with EF Core
- [x] Convert reference data SPs to EF queries (ChronicIllness, Country, Gender, Title, etc.)
- [x] Build DTOs for API models
- [x] Configure AutoMapper for entity-DTO mapping
- [x] Implement Unit of Work pattern for transactions
- [ ] Write unit tests for repository layer

**SP Conversion Strategy:**
| Category | Count (Est.) | Approach |
|----------|--------------|----------|
| Simple Reference Data Select | ~60 | Convert to EF LINQ |
| Single Table CRUD | ~80 | Convert to EF Core |
| Complex Multi-table Joins | ~40 | Convert to EF with includes |
| Business Logic Heavy | ~50 | Keep as SP, call via FromSqlRaw |
| Audit/Triggers | ~20 | Keep as SP |
| Batch Operations | ~28 | Keep as SP, evaluate later |

**Deliverable:** Repository layer with reference data access

---

### Phase 4: API Layer - Reference Data (Week 5)
- [ ] Build API controllers for lookup/reference data
- [ ] Implement pagination, filtering, sorting
- [ ] Add Swagger/OpenAPI documentation
- [ ] Create Angular services for API communication
- [ ] Build reusable Angular components (dropdowns, lookups)

**Deliverable:** Reference data API + Angular components

---

### Phase 5: Case Management Module (Weeks 6-9) 🔥 CRITICAL PATH
- [ ] Convert Case Management SPs to EF Core:
  - `usp_Case_Select`, `usp_Case_Insert`, `usp_Case_Update`, `usp_Case_Delete`
  - `usp_Case_CPT_*`, `usp_Case_ICD_*`, `usp_Case_Tariff_*`
  - `usp_Case_FacilityType_*`, `usp_Case_Exclusion_*`
- [ ] Build Case API controllers with full CRUD
- [ ] Implement Angular Case module with routing
- [ ] Create Case list/search component with ag-Grid
- [ ] Build Case form with reactive forms (multiple tabs):
  - Primary details (member, medical aid, dates)
  - CPT codes (procedure codes)
  - ICD codes (diagnosis codes)
  - Tariffs and pricing
  - Facility types
  - Exclusions
- [ ] Implement case linking functionality
- [ ] Add case notes and comments UI
- [ ] Create case letter generation
- [ ] Test Case workflows end-to-end vs legacy system

**Deliverable:** Complete Case Management module (primary business function)

---

### Phase 6: Member Management Module (Weeks 9-10)
- [ ] Convert Member-related stored procedures
- [ ] Build Member API controllers
- [ ] Create Angular Member module
- [ ] Implement member search/lookup with advanced filters
- [ ] Build member registration form
- [ ] Add medical aid product association
- [ ] Implement chronic illness tracking

**Deliverable:** Member Management module

---

### Phase 7: Finance & Billing Module (Weeks 11-13) 🔥 CRITICAL PATH
- [ ] Convert Finance SPs (`usp_Case_Billing_*`, `usp_Remittance_*`)
- [ ] Build Finance API with billing, payment, discount endpoints
- [ ] Create Finance Angular module
- [ ] Implement billing data capture form
- [ ] Build bulk payment processing UI
- [ ] Add remittance tracking and reconciliation
- [ ] Implement discount management (case & provider level)
- [ ] Create billing status workflow UI

**Deliverable:** Finance & Billing module

---

### Phase 8: Tariff Management Module (Weeks 13-15)
- [ ] Convert Tariff stored procedures (complex pricing logic)
- [ ] Build Tariff API with versioning support
- [ ] Create Tariff Management Angular module
- [ ] Implement base tariff administration
- [ ] Build tariff period management
- [ ] Add service provider tariff customization
- [ ] Create tariff code lookup with search

**Deliverable:** Tariff Management module

---

### Phase 9: Additional Modules (Weeks 15-17)
- [ ] **Bookings Module** (API + Angular)
- [ ] **Service Provider Management** (API + Angular)
- [ ] **Medical Aid Administration** (API + Angular)
- [ ] **Code Lookup Module** (CPT, ICD, NAPPI)
- [ ] **Exclusions Management** (API + Angular)
- [ ] **System Administration** (user management, system data)

**Deliverable:** All supporting modules

---

### Phase 10: Reporting with jsreport (Weeks 17-19)
- [ ] Set up jsreport server (Docker or Windows service)
- [ ] Analyze 15+ SSRS reports from legacy system
- [ ] Convert SSRS reports to jsreport templates (HTML/Handlebars):
  - Case reports (between dates, admit dates, exports)
  - Financial reports (WIP extract, billing summaries)
  - Case letters
  - Booking reports
  - Custom exports
- [ ] Create Angular reporting service with jsreport API
- [ ] Implement report viewer component
- [ ] Add report parameter forms
- [ ] Implement report scheduling/email functionality

**jsreport Setup:**
```bash
# Docker option
docker run -p 5488:5488 jsreport/jsreport

# Or install as Windows service
npm install -g @jsreport/jsreport-cli
jsreport init
jsreport configure --template handlebars
```

**Deliverable:** Complete reporting system

---

### Phase 11: Data Import & ETL (Weeks 19-20)
- [ ] Convert file import stored procedures
- [ ] Build API endpoints for data imports:
  - NAPPI codes
  - Billing files
  - DRD member files
- [ ] Create Angular file upload components
- [ ] Implement import validation and error reporting
- [ ] Build import history tracking
- [ ] Add data transformation/mapping UI

**Deliverable:** Data import functionality

---

### Phase 12: Testing & Validation (Weeks 20-22)
- [ ] Write comprehensive API integration tests
- [ ] Create Angular E2E tests with Cypress/Playwright
- [ ] Perform parallel data entry testing (legacy vs new)
- [ ] Compare SP output vs EF queries for consistency
- [ ] Load testing for critical endpoints (target: 50 concurrent users)
- [ ] Security testing (penetration, authorization)
- [ ] Multi-tenant data isolation testing
- [ ] User acceptance testing with stakeholders

**Test Coverage Goals:**
- API: 80% code coverage
- Angular services: 70% coverage
- E2E: All critical workflows (10+ scenarios)

**Deliverable:** Production-ready tested system

---

### Phase 13: Deployment & Migration (Weeks 22-24)
- [ ] Configure IIS for Angular SPA and API hosting
- [ ] Set up application pools with .NET CLR
- [ ] Configure URL rewrite rules for Angular routing
- [ ] Set up jsreport server on-premises
- [ ] Configure SSL certificates
- [ ] Create deployment scripts/automation
- [ ] Set up logging and monitoring (Serilog, Application Insights)
- [ ] Perform staged rollout with pilot users
- [ ] Create user training materials and documentation
- [ ] Execute cutover plan with data validation
- [ ] Implement rollback procedures

**Deliverable:** Production deployment with rollback capability

---

## 🔑 Key Technical Specifications

### Database
- **Connection**: Shared SQL Server database with legacy system
- **Multi-tenancy**: Global query filter on `MainClientId` in EF Core
- **Migrations**: EF Core migrations for new tables only, existing schema unchanged
- **Performance**: Benchmark critical queries, use stored procedures where EF is slower

### API Architecture
```
MedManage.API (Controllers, DTOs, Middleware)
     ↓ depends on
MedManage.Core (Domain Models, Interfaces, Business Logic)
     ↓ depends on
MedManage.Infrastructure (EF Core, Repositories, External Services)
```

### Authentication Flow
```
Angular App → API Auth Endpoints (Login/Register/Refresh)
                         ↓
                    Validate against aspnet_Membership
                         ↓
                    JWT Token (with MainClientId claim)
                         ↓
API Request → JWT Middleware → Validate Token → Set DbContext Filter
```

**Token Strategy:**
- Access Token: 15 minutes (short-lived)
- Refresh Token: 7 days (long-lived, stored in database)
- Automatic refresh on 401 errors via interceptor
- Session timeout: 30 minutes (configurable)

### Angular Architecture
```
client/medmanage-angular/
├── src/
│   ├── app/
│   │   ├── core/              # Singleton services, guards, interceptors
│   │   ├── shared/            # Reusable components, directives, pipes
│   │   ├── features/
│   │   │   ├── case-management/
│   │   │   ├── finance/
│   │   │   ├── member/
│   │   │   ├── tariff/
│   │   │   └── ...
│   │   ├── state/             # NgRx store, actions, reducers, effects
│   │   └── models/            # TypeScript interfaces
```

## 📦 Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Frontend** | Angular | 17+ |
| | TypeScript | 5.3+ |
| | NgRx (State Management) | 17+ |
| | Angular Material | 17+ |
| | ag-Grid / PrimeNG | Latest |
| **Backend** | .NET | 8.0 |
| | ASP.NET Core Web API | 8.0 |
| | Entity Framework Core | 8.0 |
| | AutoMapper | 12+ |
| **Authentication** | JWT with aspnet_Membership | Built-in |
| | JWT Bearer | Built-in |
| **Database** | SQL Server | 2019+ |
| **Reporting** | jsreport | 3.11+ |
| **Testing** | xUnit | 2.6+ |
| | Moq | 4.20+ |
| | Cypress / Playwright | Latest |
| **DevOps** | IIS | 10.0+ |
| | Git | Latest |

## 🚀 Getting Started

### Prerequisites
- Node.js 18+ and npm
- .NET 8 SDK
- SQL Server 2019+
- Visual Studio 2022 or VS Code
- Angular CLI: `npm install -g @angular/cli`

### Initial Setup

1. **Clone and Navigate:**
   ```bash
   cd c:\Code\MedManage\MedManage\MedManage.Modern
   ```

2. **Restore .NET Dependencies:**
   ```bash
   dotnet restore
   ```

3. **Install Angular Dependencies:**
   ```bash
   cd client/medmanage-angular
   npm install
   ```

4. **Configure Connection String:**
   Edit `src/MedManage.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true"
     }
   }
   ```

5. **Run API:**
   ```bash
   cd src/MedManage.API
   dotnet run
   ```
   API will be available at `https://localhost:5001`

6. **Run Angular App:**
   ```bash
   cd client/medmanage-angular
   ng serve
   ```
   App will be available at `http://localhost:4200`

## 📝 Development Guidelines

### Branching Strategy
- `main` - Production-ready code
- `develop` - Integration branch
- `feature/module-name` - Feature branches
- `hotfix/issue-description` - Emergency fixes

### Commit Messages
```
feat: Add case management list component
fix: Resolve multi-tenant filter in repository
docs: Update API documentation for Finance module
test: Add integration tests for Member API
refactor: Simplify tariff calculation logic
```

### Code Review Checklist
- [ ] All tests passing
- [ ] API endpoints documented in Swagger
- [ ] Angular components follow style guide
- [ ] Security: Authorization checks in place
- [ ] Performance: No N+1 queries
- [ ] Multi-tenancy: MainClientId filter applied

## 🔍 Stored Procedures Analysis

Full analysis available in: `docs/stored-procedures-analysis.md`

**Summary:**
- **278+ stored procedures** to analyze
- **Priority groups:**
  1. Reference data (immediate conversion) - ~60 SPs
  2. Simple CRUD (convert to EF) - ~80 SPs
  3. Complex queries (evaluate case-by-case) - ~90 SPs
  4. Keep as SPs (performance/complexity) - ~48 SPs

**Conversion Template:**
```csharp
// Before: Stored Procedure Call
var result = db.ExecuteDataSet(
    db.GetStoredProcCommand("usp_Member_Select"),
    memberIdParam
).Tables[0];

// After: EF Core Query
var result = await _context.Members
    .Where(m => m.MemberId == memberId && m.MainClientId == _currentClientId)
    .Include(m => m.MedicalAidProduct)
    .ToListAsync();
```

## 📊 Progress Tracking

| Module | Status | API Complete | UI Complete | Tests | Notes |
|--------|--------|--------------|-------------|-------|-------|
| **Foundation** | � Complete | 100% | 100% | 80% | Core structure |
| **Authentication** | 🔵 Complete | 100% | 100% | 85% | JWT + Refresh Tokens |
| **Reference Data** | ⚪ Not Started | 0% | 0% | 0% | Phase 3 - Next |
| **Case Management** | ⚪ Not Started | 0% | 0% | 0% | Phase 5 - Critical |
| **Member** | ⚪ Not Started | 0% | 0% | 0% | Phase 6 |
| **Finance** | ⚪ Not Started | 0% | 0% | 0% | Phase 7 - Critical |
| **Tariff** | ⚪ Not Started | 0% | 0% | 0% | Phase 8 |
| **Bookings** | ⚪ Not Started | 0% | 0% | 0% | Phase 9 |
| **Reporting** | ⚪ Not Started | 0% | 0% | 0% | Phase 10 |
| **Data Import** | ⚪ Not Started | 0% | 0% | 0% | Phase 11 |

**Legend:** 🟢 In Progress | 🟡 Blocked | 🔵 Complete | ⚪ Not Started

## 🎯 Success Metrics

- [ ] All core workflows functional in new system
- [ ] Performance: Sub-second response for 95% of operations
- [ ] Zero data loss during parallel operation
- [ ] User acceptance rate >80% after 2 weeks
- [ ] All 15+ reports rendering correctly in jsreport
- [ ] 50+ concurrent users supported
- [ ] 80%+ code coverage for critical paths

## 📚 Additional Resources

- **Legacy Codebase:** `../MedManage/` folder
- **Database Scripts:** `../MedManage/Tables.sql`, `../MedManage/Views and stored procedures.sql`
- **Legacy Forms Reference:** `../MedManage/*.cs` (58 forms for UI requirements)
- **Business Logic:** `../MedManageLib/*.cs` (reference for API implementation)

## 🤝 Contributing

1. Create feature branch from `develop`
2. Implement changes with tests
3. Update documentation
4. Submit pull request with description
5. Pass code review
6. Merge to `develop`

## 📞 Support

For questions or issues during migration, contact the development team.

---

**Last Updated:** April 16, 2026  
**Project Status:** Phase 1 - Foundation Setup
