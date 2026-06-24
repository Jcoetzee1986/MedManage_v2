# Design Decisions

## Architecture Pattern

Each module follows the same vertical slice:
```
Controller (API) → Service (Business Logic) → Repository (Data Access)
     ↕                    ↕                         ↕
   DTOs              Interfaces                 EF Core DbContext
```

## SP Conversion Strategy

| Category | Approach | Example |
|----------|----------|---------|
| Simple CRUD | Convert to EF Core LINQ | Member, reference data |
| Complex search | Dynamic IQueryable building | Case search with 14 filters |
| Tariff calculations | Keep as SP, call via FromSqlRaw | fn_sc_TotalTariffPerCase |
| Business-heavy logic | Keep as SP, wrap in service | Billing reconciliation |
| Reports | Keep temporarily, migrate to jsreport | rpt_Cases_BetweenDates |
| ETL/Import | Keep as SP, expose via API | Import.usp_Members_DRD_* |

## Sub-Entity API Pattern

For case-related sub-entities, use nested routes:
```
GET    /api/cases/{caseId}/cpt         → List CPT codes for case
POST   /api/cases/{caseId}/cpt         → Add CPT code to case
PUT    /api/cases/{caseId}/cpt/{id}    → Update CPT code
DELETE /api/cases/{caseId}/cpt/{id}    → Remove CPT code
```

## File/Document Storage

- Store file metadata in database (LinkedFile table)
- Store actual files on disk (configurable path via appsettings)
- Serve via streaming endpoint with content-type detection
- Generate thumbnails server-side for images

## Concurrency (replacing desktop session locking)

Legacy used Session_User_Case table to prevent concurrent edits.
Modern approach: optimistic concurrency with ETag/rowversion + optional
SignalR notification when another user opens the same case.
