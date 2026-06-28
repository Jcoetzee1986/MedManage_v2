# MedManage Modernization — Gap Analysis & Completion Spec

## Executive Summary

This spec compares the legacy WinForms MedManage application against the modern Angular/.NET rewrite (MedManage.Modern) to identify all gaps that must be closed for feature parity.

---

## Current State (What's Done)

| Area | Status | Notes |
|------|--------|-------|
| Solution structure | ✅ Complete | API, Core, Infrastructure, Shared projects |
| Authentication | ✅ Complete | JWT, refresh tokens, password reset, session timeout |
| Health checks | ✅ Complete | DB connectivity verification |
| Entity scaffolding | ✅ Complete | 106 entities from 84 tables |
| Repository pattern | ~90% | 49 repos, ~20 build errors remain |
| Member CRUD API | ✅ Complete | GET, POST, PUT, DELETE, Search working |
| Angular foundation | ✅ Complete | Auth components, Material UI, routing |
| Logging (Serilog/Graylog) | ✅ Complete | Structured JSON, GELF integration |

---

## Gaps Identified (Legacy → Modern)

### GAP 1: Build Stability (BLOCKING)

**Legacy:** Compiles and runs without errors
**Modern:** ~20 build errors in repository layer (property name mismatches, missing navigation properties)

**Requirements:**
- REQ-1.1: Fix remaining build errors in all 49 repository implementations
- REQ-1.2: Achieve zero-error build of MedManage.Modern.sln
- REQ-1.3: Fix remaining 24 audit triggers broken by DateDeleted migration
- REQ-1.4: Implement proper soft delete (DateDeleted filter in DbContext, update Repository.DeleteAsync)
- REQ-1.5: Replace hardcoded "system" UserID with authenticated user context (ICurrentUserService)

---

### GAP 2: Case Management Module (CRITICAL — primary business function)

**Legacy:** Case.cs (3371 lines), CaseManagement.cs (789 lines) — full CRUD with:
- Case create/edit with multi-tab form (member, admission/discharge, codes, tariffs, billing)
- Case search with 14+ filter parameters (auth number, member, dates, practice, status, ICD, CPT)
- Case copy (duplicate case to new)
- Case linking (parent/child relationships)
- Case notes with interim amounts (8 cost categories: Hospital, Radiology, Dialysis, Specialist, Physio, Transport, Accommodation, Script)
- Case comments (separate from notes)
- Case checklist (template-based, checked/not-applicable/comments)
- Case letter notes (correspondence with discharge/referral options)
- Case-linked documents (file upload/download)
- Case NAPPI codes (medication tracking with dispensary/ward/theater/TTO flags)
- Case facility types (days-in-care: admission/discharge per ward, LOS, ventilator minutes)
- Case exclusions (with comments)
- Case CPT codes (procedure codes with primary/secondary flags, date of procedure)
- Case ICD codes (diagnosis codes with primary/secondary/co-morbidity flags)
- Case tariffs (with value, quantity, agreed rate override, rejected flag, ValueIsTotal)
- Duplicate detection (same member + provider + admission date)
- Session locking (prevent concurrent edits)
- Case status workflow (Booking → Active Case with ChangeToCaseDate)
- Case category classification
- Penalty percentage tracking
- WCA/IOD flag (Workers Compensation)

**Modern Status:** CaseService exists (basic CRUD + search) but case sub-entities not implemented

**Requirements:**
- REQ-2.1: Case API — full CRUD (create, read, update, soft-delete) with all 26 case fields
- REQ-2.2: Case search API with filters (auth number, member, dates, practice, medical aid, ICD, CPT, status, case type)
- REQ-2.3: Case CPT sub-entity API (CRUD, list by case)
- REQ-2.4: Case ICD sub-entity API (CRUD, list by case)
- REQ-2.5: Case Tariff sub-entity API (CRUD, list by case) — calls Tariff schema SPs
- REQ-2.6: Case Facility Type sub-entity API (CRUD, list by case)
- REQ-2.7: Case Exclusion sub-entity API (CRUD, list by case)
- REQ-2.8: Case Notes API (CRUD with 8 interim cost categories)
- REQ-2.9: Case Comments API (CRUD)
- REQ-2.10: Case Checklist API (CRUD from template, checked/not-applicable/comments)
- REQ-2.11: Case Link API (create parent/child link, list linked cases, unlink)
- REQ-2.12: Case Linked Files API (upload, download, list, delete)
- REQ-2.13: Case NAPPI Codes API (CRUD with dispensary/ward/theater/TTO flags)
- REQ-2.14: Case Letter Notes API (CRUD with include discharge/referral flags)
- REQ-2.15: Case copy endpoint (deep copy with configurable fields)
- REQ-2.16: Duplicate case detection endpoint
- REQ-2.17: Case session locking (prevent concurrent edits)
- REQ-2.18: Angular Case module — list/search, detail form with tabbed sub-entities

---

### GAP 3: Service Provider Module

**Legacy:** ServiceProviderLookup.cs, ServiceProvider.cs, SharedObjects.cs
- Full CRUD with 35+ fields (addresses, banking, speciality, tariff structure)
- Search by name, surname, practice name, practice number
- Autocomplete lookup
- Visibility toggle
- Practice number uniqueness validation
- Tariff assignment (ServiceProvider_Tariff, ServiceProvider_Tariff_Custom)
- Provider discount management (ServiceProvider_MainClient_Discount)

**Modern Status:** ServiceProviderService exists (basic CRUD + search)

**Requirements:**
- REQ-3.1: Service provider tariff assignment API (CRUD ServiceProvider_Tariff)
- REQ-3.2: Service provider custom tariff API (CRUD ServiceProvider_Tariff_Custom)
- REQ-3.3: Service provider discount API (per MainClient)
- REQ-3.4: Practice number uniqueness validation endpoint
- REQ-3.5: Autocomplete/type-ahead search endpoint
- REQ-3.6: Angular Service Provider module (search, detail form, tariff management)

---

### GAP 4: Member Management Module

**Legacy:** MemberLookup.cs, MemberAdd.cs, SharedObjects.cs
- Full CRUD with 48+ fields
- Search by surname, name, member number, passport, ID number, DOB
- Chronic illness tracking (many-to-many: Member_ChronicIllness)
- Medical aid product history (Member_MedicalAidProduct with date ranges)
- Member notes (free text with date/user)
- Suspended/deceased status with dates
- Medical aid exhausted tracking
- "AllowServices" validation from MedicalAidProduct
- Member number uniqueness validation

**Modern Status:** MemberService complete (CRUD + search), but sub-entities missing

**Requirements:**
- REQ-4.1: Member chronic illness API (CRUD, list by member)
- REQ-4.2: Member medical aid product history API (CRUD with date ranges)
- REQ-4.3: Member notes API (CRUD)
- REQ-4.4: Member number uniqueness validation endpoint
- REQ-4.5: Member AllowServices validation (check product before case creation)
- REQ-4.6: Angular Member module (search, detail form, chronic illness tab, notes tab)

---

### GAP 5: Finance & Billing Module

**Legacy:** Finance.cs, FinanceDataCapture.cs, FinanceBulkPayment.cs
- Billing CRUD (Case_Billing: accounts, invoices, dates, amounts, status)
- Billing status workflow (received → submitted → paid)
- Bulk payment processing (UpdateToPaid with amount, date, comments)
- Remittance tracking (UpdateRemittanceNumber)
- Account number autocomplete by provider
- Duplicate account checking
- Billing summary per case
- Cases by remittance lookup
- Case discount management (finance.Case_Discount)
- Provider discount management (finance.ServiceProvider_MainClient_Discount)
- Final invoice amount tracking with update timestamps
- WIP (Work In Progress) extract reporting

**Modern Status:** Not implemented

**Requirements:**
- REQ-5.1: Case Billing API (full CRUD: create, update, delete, list by case)
- REQ-5.2: Billing search by provider/date range/account number
- REQ-5.3: Bulk payment endpoint (mark multiple billings as paid)
- REQ-5.4: Remittance update endpoint
- REQ-5.5: Duplicate account number detection
- REQ-5.6: Billing summary endpoint per case
- REQ-5.7: Cases by remittance lookup
- REQ-5.8: Case discount API (CRUD)
- REQ-5.9: Provider discount API (CRUD per MainClient)
- REQ-5.10: Angular Finance module (billing data capture, bulk payment, remittance view)

---

### GAP 6: Tariff System

**Legacy:** TariffManagement.cs, TariffCodeLookup.cs, BaseTariffAdd.cs, NewTariff.cs, CustomiseServiceProviderTariff.cs, fn_sc_TotalTariffPerCase (200+ line SQL function)
- Base tariff management (BaseTariff: code, speciality, description, date range)
- Tariff rates by period (Tariff: amount, metric, quantity, dates, TariffPeriodName)
- Tariff name/structure management
- Service provider tariff assignment (percentage-based adjustments)
- Custom provider tariff overrides (specific amount per code per provider)
- Complex tariff calculation logic (fn_sc_TotalTariffPerCase):
  - Provider rate vs normal rate vs scheme rate vs agreed rate override
  - Quantity × rate × VAT (if TariffInclVAT)
  - MaxUnits logic
  - Discount calculation (case-level + provider-level)
  - Overcharged detection
- Tariff lookup by code + provider + treatment date
- Medical aid tariff association (MedicalAid_Tariff)
- MainClient tariff configuration (MainClient_Tariff)

**Modern Status:** Not implemented (SP conversion matrix recommends keeping as SPs)

**Requirements:**
- REQ-6.1: Tariff lookup API (by code + provider + treatment date — wraps SP)
- REQ-6.2: Tariff calculation API (wraps fn_sc_TotalTariffPerCase or equivalent SP)
- REQ-6.3: Base tariff management API (CRUD)
- REQ-6.4: Tariff rate management API (CRUD per period)
- REQ-6.5: Tariff name management API (CRUD)
- REQ-6.6: Medical aid tariff association API
- REQ-6.7: Angular Tariff module (tariff administration, code lookup)

---

### GAP 7: Bookings Module

**Legacy:** Bookings.cs, BookingAdd.cs, SharedObjects.cs
- Booking CRUD (linked to member, travel dates)
- Search by surname, name, member number, travel date range
- All bookings by member number
- Booking linked to case (case HasBooking flag, Booking→Case conversion)

**Modern Status:** Not implemented

**Requirements:**
- REQ-7.1: Bookings API (full CRUD)
- REQ-7.2: Booking search by filters (member, dates)
- REQ-7.3: Booking-to-case conversion workflow
- REQ-7.4: Angular Bookings module

---

### GAP 8: Medical Aid Management

**Legacy:** MedicalAid.cs, MedicalAidExclusions.cs, SharedObjects.cs
- Medical aid CRUD (name, dates, CasePrefix, ReportTemplate)
- Medical aid product management (AllowServices flag)
- Medical aid exclusion management (exclude specific BaseTariff codes)
- Medical aid tariff association
- MainClient_MedicalAidProduct sorting/assignment

**Modern Status:** Not implemented

**Requirements:**
- REQ-8.1: Medical Aid API (full CRUD)
- REQ-8.2: Medical Aid Product API (CRUD, AllowServices flag)
- REQ-8.3: Medical Aid Exclusion API (CRUD, link BaseTariff codes)
- REQ-8.4: Medical Aid Tariff association API
- REQ-8.5: Angular Medical Aid module

---

### GAP 9: Reference Data APIs

**Legacy:** SharedObjects.cs — full CRUD for all lookup tables
- ChronicIllness, Country, Exclusion, Gender, Language, MarritalStatus
- MemberStatus, PeriodInCountry, Race, Speciality, Title
- FacilityType, CaseStatus, CaseType, CaseCategory, ChecklistTemplate
- BillingStatus, SuspendedReason, TariffStructure

**Modern Status:** Repositories exist but no API controllers or DTOs for reference data

**Requirements:**
- REQ-9.1: Generic reference data API controllers (CRUD for all ~18 lookup tables)
- REQ-9.2: Angular shared dropdown/lookup service consuming reference data APIs
- REQ-9.3: Admin screen for managing reference data (add/edit/delete lookups)

---

### GAP 10: Episodes Module

**Legacy:** CaseManagement.cs — episode management
- Episode CRUD (description, member, date)
- Link cases to episodes (Episode_Case junction table)
- View all cases in an episode

**Modern Status:** Not implemented

**Requirements:**
- REQ-10.1: Episode API (CRUD)
- REQ-10.2: Episode-Case linking API (add/remove cases)
- REQ-10.3: List cases by episode endpoint

---

### GAP 11: Reporting

**Legacy:** 15+ report forms (rpt_*.cs) using Crystal Reports/RDLC
- Cases between dates, cases by admission dates, cases by primary/parent
- Billing summary, billing by case, billing by member
- Case letter (authorization letter per medical aid)
- Case comment/update export
- Case tariff detail
- Finance cases, WIP extract
- My cases export, print linked cases

**Modern Status:** Not implemented (planned for jsreport in Phase 10)

**Requirements:**
- REQ-11.1: jsreport server integration (setup, API connection)
- REQ-11.2: Case letter report template (per medical aid ReportTemplate)
- REQ-11.3: Cases between dates report
- REQ-11.4: WIP/financial extract report
- REQ-11.5: Billing summary reports
- REQ-11.6: Case tariff detail report
- REQ-11.7: Angular report viewer component with parameter forms
- REQ-11.8: Report export (PDF, Excel)

---

### GAP 12: Data Imports

**Legacy:** FileImports.cs, ImportDRDEmployeeFiles.cs, ImportNappiCodes.cs
- DRD member file import (CSV → staging → merge into Member table)
- NAPPI code import (pharmaceutical codes with pricing and dates)
- Billing file import (Import.BillingFiles with practice name matching)

**Modern Status:** Not implemented

**Requirements:**
- REQ-12.1: DRD member file import API (upload CSV, validate, merge)
- REQ-12.2: NAPPI code import API (upload, validate, upsert)
- REQ-12.3: Billing file import API (with practice name matching)
- REQ-12.4: Import history/status tracking
- REQ-12.5: Angular import module (file upload, progress, validation errors)

---

### GAP 13: Image/Document Management

**Legacy:** Images.cs, Gallery.cs, uc_Gallery.cs
- Case-linked images (store in database as binary, resize/thumbnail)
- Case-linked files (file path references, upload/download)
- Image gallery view

**Modern Status:** Not implemented

**Requirements:**
- REQ-13.1: File upload API (case-linked documents)
- REQ-13.2: File download/streaming API
- REQ-13.3: Image thumbnail generation (server-side resize)
- REQ-13.4: Angular document viewer/gallery component

---

### GAP 14: System Administration

**Legacy:** SystemData.cs, UserMaintenance.cs, DeleteUserSession.cs, ChooseClient.cs
- System configuration (email, addresses, logos, default facility types, VAT)
- User management (create, approve, lock/unlock, assign roles)
- Active session management (force-close)
- Multi-client selection at login

**Modern Status:** Auth endpoints exist but no admin management

**Requirements:**
- REQ-14.1: System data API (CRUD system configuration)
- REQ-14.2: User management API (list users, approve, lock/unlock, assign roles)
- REQ-14.3: Active sessions API (list, force-terminate)
- REQ-14.4: Angular admin module

---

### GAP 15: Role-Based Access Control

**Legacy:** 5 roles controlling UI visibility and actions:
- System Administrator, Metadata Administrator, Case Manager, Billing Auditing, Imports

**Modern Status:** JWT includes roles, but no controller-level authorization enforcement

**Requirements:**
- REQ-15.1: Apply [Authorize(Roles = "...")] to all API controllers
- REQ-15.2: Angular route guards per module based on user roles
- REQ-15.3: UI element visibility based on roles (show/hide buttons, tabs)

---

### GAP 16: Code Lookup (CPT, ICD, NAPPI)

**Legacy:** CodeLookup.cs, SharedObjects.cs
- CPT code search by code or description
- ICD code search by code or description
- NAPPI code search by code, description, date
- Used in case forms for adding medical codes

**Modern Status:** Not implemented as API

**Requirements:**
- REQ-16.1: CPT code search API (by code, description)
- REQ-16.2: ICD code search API (by code, description)
- REQ-16.3: NAPPI code search API (by code, description, effective date)
- REQ-16.4: Angular reusable code lookup component (typeahead/dialog)

---

### GAP 17: Business Rules & Validation

**Legacy:** Embedded in Case.cs form logic
- Member AllowServices check before case save
- Medical aid prefix auto-update when member changes medical aid
- Suspended/deceased/exhausted member warnings
- Case status workflow constraints
- Duplicate case detection
- Discharge date must be >= latest facility discharge
- Admission date must be <= earliest facility admission
- Case type and category required before save
- Member/provider required before save
- Account number edit restricted by role

**Modern Status:** FluentValidation on Member (basic field validation only)

**Requirements:**
- REQ-17.1: Business rule validation service for case create/update
- REQ-17.2: Member status warnings API (suspended, deceased, exhausted, AllowServices)
- REQ-17.3: Case date consistency validation (admission/discharge vs facility types)
- REQ-17.4: Auth number prefix management (auto-update on medical aid change)

---

## Priority & Sequencing

### Phase A — Stabilize (1-2 days)
- REQ-1.1 through REQ-1.5 (fix build, soft delete, user context)

### Phase B — Reference Data APIs (1 week)
- REQ-9.1 through REQ-9.3

### Phase C — Case Management Deep Dive (3-4 weeks)
- REQ-2.1 through REQ-2.18

### Phase D — Finance & Billing (2 weeks)
- REQ-5.1 through REQ-5.10

### Phase E — Member Sub-entities + Service Provider Enhancements (1 week)
- REQ-4.1 through REQ-4.6
- REQ-3.1 through REQ-3.6

### Phase F — Tariff System (2 weeks)
- REQ-6.1 through REQ-6.7

### Phase G — Supporting Modules (2 weeks)
- REQ-7.1 through REQ-7.4 (Bookings)
- REQ-8.1 through REQ-8.5 (Medical Aid)
- REQ-10.1 through REQ-10.3 (Episodes)
- REQ-16.1 through REQ-16.4 (Code Lookups)

### Phase H — Cross-cutting Concerns (1 week)
- REQ-15.1 through REQ-15.3 (RBAC)
- REQ-17.1 through REQ-17.4 (Business rules)
- REQ-14.1 through REQ-14.4 (Admin)

### Phase I — Reporting & Imports (3 weeks)
- REQ-11.1 through REQ-11.8
- REQ-12.1 through REQ-12.5
- REQ-13.1 through REQ-13.4

---

## Out of Scope (Legacy features NOT migrated)

- Auto-update mechanism (AppUpdates/ClientUpdates) — not needed for web app
- VPN/network connectivity checks — not needed for web deployment
- Desktop gallery/image manipulation (server-side thumbnails replace this)
- Crystal Reports / RDLC — replaced by jsreport
- Enterprise Library 5.0 data access — replaced by EF Core
- Session_User_Case (desktop locking) — replaced by web optimistic concurrency or SignalR

---

## Success Criteria

1. All legacy forms' functionality accessible via Angular UI
2. All 278 stored procedures either converted to EF Core or wrapped via FromSqlRaw
3. Sub-second response for 95% of API operations
4. Role-based access enforced at API and UI levels
5. Data integrity maintained (same validations as legacy)
6. Reports output identical data to SSRS equivalents
7. Zero data loss during parallel operation with legacy system
