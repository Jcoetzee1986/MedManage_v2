# Implementation Plan

## Overview

This plan implements the MedManage Modernization gap closure across 9 phases, starting with build stabilization and progressing through all business modules to achieve feature parity with the legacy WinForms application.

## Tasks

- [x] Fix remaining build errors in repository implementations
  - Fix Case.Status vs CaseStatus navigation property references
  - Fix Race, ChecklistTemplate, NappiCode property name mismatches
  - Remove non-existent Include() calls (EpisodeCases, CaseBillings)
  - Fix Case.ServiceProviderId to ReferToId/ReferFromId
  - Verify build: dotnet build MedManage.Modern.sln with 0 errors

- [x] Implement proper soft delete pattern
  - Update Repository.DeleteAsync to set DateDeleted instead of hard delete
  - Add global query filter in DbContext for DateDeleted == null
  - Add includeDeleted parameter to search methods for admin views
  - Test: deleted records excluded from queries, restorable

- [x] Fix audit triggers for remaining 24 tables
  - Create SQL script that ALTERs all 24 triggers with explicit column names
  - Ensure INSERT/UPDATE/DELETE work on all major tables
  - Document which triggers were updated

- [x] Wire up ICurrentUserService for audit fields
  - Replace hardcoded system with authenticated user from JWT claims
  - Ensure DateInserted/UserID set on create, DateUpdated/UpdatedUserID on update
  - Test with authenticated request

- [x] Create generic reference data controller pattern
  - Create base ReferenceDataController with TEntity and TDto generics
  - Endpoints: GET all, GET by ID, POST, PUT, DELETE
  - Apply to Gender, Language, Race, Title, MarritalStatus, MemberStatus
  - Apply to PeriodInCountry, Country, Speciality, FacilityType
  - Apply to CaseStatus, CaseType, CaseCategory, ChecklistTemplate
  - Apply to BillingStatus, Exclusion, ChronicIllness, SuspendedReason

- [x] Angular reference data service and admin UI
  - Create shared reference data service (cacheable)
  - Create admin screen for CRUD on all lookup tables
  - Create reusable dropdown component that consumes ref data APIs

- [x] Case API core CRUD and search
  - Create CaseDto, CreateCaseRequest, UpdateCaseRequest, CaseSearchRequest
  - Implement CaseService with all 26+ case fields
  - Implement search with filters (auth number, member, dates, practice, status, ICD, CPT)
  - Create CasesController with GET, POST, PUT, DELETE, POST/search
  - Include duplicate detection endpoint

- [x] Case sub-entity APIs for CPT codes
  - DTOs: CaseCptDto, CreateCaseCptRequest
  - Endpoint: GET/POST/PUT/DELETE /api/cases/caseId/cpt
  - Fields: CPTID, DateOfProcedure, PrimaryCode, SecondaryCode

- [x] Case sub-entity APIs for ICD codes
  - DTOs: CaseIcdDto, CreateCaseIcdRequest
  - Endpoint: GET/POST/PUT/DELETE /api/cases/caseId/icd
  - Fields: ICDID, DateOfProcedure, PrimaryCode, SecondaryCode, CoMorbidityCode

- [x] Case sub-entity APIs for Tariffs
  - DTOs: CaseTariffDto, CreateCaseTariffRequest
  - Endpoint: GET/POST/PUT/DELETE /api/cases/caseId/tariffs
  - Fields: TariffID, Value, Qty, AgreedRateOverride, ValueIsTotal, Rejected, DateOfProcedure
  - Calls Tariff stored procedures via FromSqlRaw

- [x] Case sub-entity APIs for Facility Types
  - DTOs: CaseFacilityTypeDto, CreateCaseFacilityTypeRequest
  - Endpoint: GET/POST/PUT/DELETE /api/cases/caseId/facility-types
  - Fields: FacilityTypeID, DateAdmitted, DateDischarged, LOS, FacilityTypeCode, MinutesOnVentilator, Comments

- [x] Case sub-entity APIs for Notes Comments and Exclusions
  - Case Notes: CRUD with 8 interim amount categories
  - Case Comments: simple CRUD (text, user, date)
  - Case Exclusions: CRUD (ExclusionID, Comment)
  - Case Checklist: CRUD from template (Checked, NotApplicable, Comments, Date)

- [x] Case sub-entity APIs for Links Documents NAPPI and Letter Notes
  - Case Link: POST/DELETE /api/cases/caseId/links (parent/child)
  - Case Linked Files: upload/download/list/delete
  - Case NAPPI: CRUD (NappiID, Value, Quantity, Dispensary, Ward, Theater, TTO, Date)
  - Case Letter Notes: CRUD (Note, IncludeDischargeForm, IncludeReferralLetter)

- [x] Case copy and session locking
  - POST /api/cases/caseId/copy with deep copy of linked entities
  - Optimistic concurrency or SignalR lock notification
  - Case status workflow endpoint (Booking to Case transition)

- [x] Angular Case Management module
  - Case list/search page with ag-Grid
  - Case detail form with tabbed layout for Primary details, Member, Provider, Dates
  - CPT codes tab, ICD codes tab, Tariffs tab
  - Facility types tab, Exclusions tab, NAPPI tab
  - Notes tab, Comments tab, Checklist tab
  - Linked documents tab, Linked cases tab
  - Case copy dialog and code lookup integration

- [x] Finance API for Billing CRUD
  - DTOs: CaseBillingDto, CreateBillingRequest, BillingSearchRequest
  - BillingService with full CRUD
  - BillingController: GET/POST/PUT/DELETE, search by provider/dates
  - Duplicate account detection
  - Billing summary per case

- [x] Finance API for Payments and Remittance
  - Bulk payment endpoint (mark as paid: amount, date, comments)
  - Remittance update endpoint
  - Cases by remittance lookup
  - Case/provider discount management endpoints

- [x] Angular Finance module
  - Billing data capture form
  - Billing search/list view
  - Bulk payment processing screen
  - Remittance tracking view

- [x] Member sub-entity APIs
  - Member chronic illness (CRUD, list by member)
  - Member medical aid product history (CRUD with date ranges)
  - Member notes (CRUD)
  - Member number uniqueness validation

- [x] Service Provider enhancements
  - Provider tariff assignment API (ServiceProvider_Tariff CRUD)
  - Provider custom tariff API (ServiceProvider_Tariff_Custom CRUD)
  - Provider discount API (per MainClient)
  - Autocomplete/typeahead endpoint

- [x] Angular Member and Provider modules
  - Member detail with chronic illness, notes, product history tabs
  - Provider detail with tariff assignment, custom tariffs, discount tabs

- [x] Tariff APIs wrapping stored procedures
  - Tariff lookup by code + provider + treatment date (wraps SP)
  - Case tariff calculation endpoint (wraps fn_sc_TotalTariffPerCase)
  - Base tariff management API (CRUD)
  - Tariff rate/period management API (CRUD)
  - Tariff name management API

- [x] Angular Tariff module
  - Tariff administration screen
  - Tariff code lookup component (used in case form)
  - Provider tariff customization screen

- [x] Bookings module
  - API: CRUD, search by member/dates
  - Booking-to-case conversion workflow
  - Angular: booking list, add/edit form

- [x] Medical Aid module
  - API: Medical Aid CRUD, Product CRUD, Exclusion CRUD, Tariff association
  - Angular: Medical aid administration screen

- [x] Episodes module
  - API: Episode CRUD, link/unlink cases
  - Angular: Episode list, case grouping view

- [x] Code Lookup APIs for CPT ICD and NAPPI
  - Search endpoints with typeahead support
  - Angular: reusable code lookup dialog component

- [x] Role-based access control
  - Apply Authorize(Roles) attributes to all controllers
  - Angular route guards per module
  - UI element show/hide based on roles

- [x] Business rule validation
  - Case validation service (member status checks, date consistency, required fields)
  - Member AllowServices check
  - Auth number prefix management

- [x] System Administration
  - System data API (configuration CRUD)
  - User management API (list, approve, lock/unlock, role assignment)
  - Angular admin module

- [x] jsreport integration
  - Set up jsreport server (Docker or Windows service)
  - Create ReportService for API communication
  - Build report templates: case letter, cases between dates, WIP extract, billing summary

- [x] Angular reporting module
  - Report viewer component
  - Parameter forms for each report type
  - PDF/Excel export

- [x] Data import module
  - DRD member file import API (upload, validate, merge)
  - NAPPI code import API
  - Billing file import API
  - Import history tracking
  - Angular: upload UI with progress and validation display

- [x] Document and image management
  - File upload API (store linked to case/member)
  - File download/streaming
  - Server-side image thumbnail generation
  - Angular: document viewer/gallery component

## Task Dependency Graph

```
Fix remaining build errors in repository implementations →
Implement proper soft delete pattern → Fix remaining build errors in repository implementations
Fix audit triggers for remaining 24 tables → Fix remaining build errors in repository implementations
Wire up ICurrentUserService for audit fields → Fix remaining build errors in repository implementations
Create generic reference data controller pattern → Implement proper soft delete pattern
Angular reference data service and admin UI → Create generic reference data controller pattern
Case API core CRUD and search → Implement proper soft delete pattern, Wire up ICurrentUserService for audit fields
Case sub-entity APIs for CPT codes → Case API core CRUD and search
Case sub-entity APIs for ICD codes → Case API core CRUD and search
Case sub-entity APIs for Tariffs → Case API core CRUD and search
Case sub-entity APIs for Facility Types → Case API core CRUD and search
Case sub-entity APIs for Notes Comments and Exclusions → Case API core CRUD and search
Case sub-entity APIs for Links Documents NAPPI and Letter Notes → Case API core CRUD and search
Case copy and session locking → Case API core CRUD and search
Angular Case Management module → Case sub-entity APIs for CPT codes, Case sub-entity APIs for ICD codes, Case sub-entity APIs for Tariffs, Case sub-entity APIs for Facility Types, Case sub-entity APIs for Notes Comments and Exclusions, Case sub-entity APIs for Links Documents NAPPI and Letter Notes, Case copy and session locking, Angular reference data service and admin UI
Finance API for Billing CRUD → Case API core CRUD and search
Finance API for Payments and Remittance → Finance API for Billing CRUD
Angular Finance module → Finance API for Billing CRUD, Finance API for Payments and Remittance, Angular reference data service and admin UI
Member sub-entity APIs → Implement proper soft delete pattern, Wire up ICurrentUserService for audit fields
Service Provider enhancements → Implement proper soft delete pattern, Wire up ICurrentUserService for audit fields
Angular Member and Provider modules → Member sub-entity APIs, Service Provider enhancements, Angular reference data service and admin UI
Tariff APIs wrapping stored procedures → Implement proper soft delete pattern, Wire up ICurrentUserService for audit fields
Angular Tariff module → Tariff APIs wrapping stored procedures, Angular reference data service and admin UI
Bookings module → Case API core CRUD and search
Medical Aid module → Create generic reference data controller pattern
Episodes module → Case API core CRUD and search
Code Lookup APIs for CPT ICD and NAPPI → Implement proper soft delete pattern
Role-based access control → Case API core CRUD and search, Finance API for Billing CRUD, Member sub-entity APIs, Service Provider enhancements
Business rule validation → Case API core CRUD and search, Member sub-entity APIs
System Administration → Wire up ICurrentUserService for audit fields
jsreport integration → Case API core CRUD and search, Finance API for Billing CRUD
Angular reporting module → jsreport integration, Angular reference data service and admin UI
Data import module → Member sub-entity APIs, Code Lookup APIs for CPT ICD and NAPPI
Document and image management → Case sub-entity APIs for Links Documents NAPPI and Letter Notes
```

## Notes

- Phase A (first 4 tasks) must complete before other work begins as it stabilizes the build
- Case management is the critical path and largest module
- Angular tasks depend on their respective API tasks
- Tariff system wraps existing stored procedures rather than reimplementing
- Case sub-entity tasks (CPT, ICD, Tariffs, Facility Types, Notes, Links, Copy) can run in parallel once Case API core task is complete
