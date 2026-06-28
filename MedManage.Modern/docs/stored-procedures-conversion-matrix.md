# Stored Procedure Conversion Matrix

**Generated:** April 22, 2026  
**Total Stored Procedures:** 278  
**Analysis Scope:** Phase 3 - Core Data Access Layer

## Executive Summary

This matrix categorizes all 278 stored procedures in the MedManage legacy system and provides conversion recommendations for the EF Core migration.

### Distribution by Schema
| Schema | Count | Purpose |
|--------|-------|---------|
| dbo | 245 | Core application logic |
| Tariff | 13 | Tariff calculations and management |
| Import | 13 | Data import/ETL operations |
| Maintenance | 3 | Database maintenance |
| Updates | 2 | Application updates |
| Overnight | 2 | Batch processing |

### Distribution by Entity (Top 20)
| Entity | Count | Complexity |
|--------|-------|------------|
| Case | 50 | High - Complex business logic |
| Tariff | 17 | High - Pricing calculations |
| Report (rpt_*) | 14 | Medium - Reporting queries |
| ServiceProvider | 13 | Medium - CRUD operations |
| MedicalAid | 12 | Medium - CRUD + validation |
| Cases | 12 | High - Complex case operations |
| Member | 10 | Medium - CRUD operations |
| Episode | 7 | Medium - Episode management |
| Bookings | 6 | Low - Simple CRUD |
| Speciality | 6 | Low - Reference data |
| ChecklistTemplate | 4 | Low - Template CRUD |
| ChronicIllness | 4 | Low - Reference data |
| Language | 4 | Low - Reference data CRUD |
| MarritalStatus | 4 | Low - Reference data CRUD |
| MemberStatus | 4 | Low - Reference data CRUD |
| PeriodInCountry | 4 | Low - Reference data CRUD |
| Race | 4 | Low - Reference data CRUD |
| Images | 4 | Low - File management |
| Import | 13 | Medium - ETL operations |
| Maintenance | 3 | N/A - Keep as SQL |

---

## Conversion Strategy Matrix

### Category 1: Convert to EF Core LINQ (Recommended: 156 procedures, 56%)

**Criteria:** Simple CRUD operations, single-table queries, standard filtering/sorting

#### 1A. Reference Data - Simple Lookups (Estimated: 40 procedures)
**Complexity:** Low  
**Priority:** High  
**Conversion Effort:** 1-2 hours each

| Stored Procedure Pattern | EF Core Approach | Example |
|--------------------------|------------------|---------|
| `usp_<Entity>_Select` | `GetAllAsync()` | `_dbContext.Genders.Where(x => !x.Deleted).ToListAsync()` |
| `usp_<Entity>_Insert` | `AddAsync()` + `SaveChangesAsync()` | Already implemented in repositories |
| `usp_<Entity>_Update` | `UpdateAsync()` + `SaveChangesAsync()` | Already implemented in repositories |
| `usp_<Entity>_Delete` | Soft delete pattern | Already implemented in repositories |

**Entities in this category:**
- ✅ Gender (4 SPs) - **CONVERTED** (via MemberService)
- ✅ Language (4 SPs) - **CONVERTED** (via MemberService)
- ✅ Race (4 SPs) - **CONVERTED** (via MemberService)
- ✅ Title (4 SPs) - **CONVERTED** (via MemberService)
- MarritalStatus (4 SPs)
- MemberStatus (4 SPs)
- PeriodInCountry (4 SPs)
- BillingStatus (4 SPs)
- CaseStatus (4 SPs)
- CaseType (4 SPs)
- FacilityType (4 SPs)
- Speciality (6 SPs)
- ChronicIllness (4 SPs)
- ChecklistTemplate (4 SPs)

**Status:** 16/40 converted (40%)

---

#### 1B. Core Entity CRUD - Standard Operations (Estimated: 60 procedures)
**Complexity:** Low-Medium  
**Priority:** High  
**Conversion Effort:** 2-4 hours each

| Entity | Procedures | Status | Notes |
|--------|------------|--------|-------|
| **Member** | 10 | ✅ **CONVERTED** | MemberService with search, pagination implemented |
| **Case** | 12 (basic) | ✅ **CONVERTED** | CaseService with search, pagination implemented |
| **ServiceProvider** | 13 | ✅ **CONVERTED** | ServiceProviderService with filters implemented |
| Bookings | 6 | 🔲 Pending | Simple CRUD + filters |
| Episode | 7 | 🔲 Pending | Episode management |
| MedicalAid | 6 (basic) | 🔲 Pending | CRUD operations |
| MedicalAidProduct | 4 | 🔲 Pending | Product management |
| Images/LinkedFile | 4 | 🔲 Pending | File management |
| Exclusions | 4 | 🔲 Pending | Exclusion management |

**Status:** 35/60 converted (58%)

---

#### 1C. Related Entity Operations (Estimated: 56 procedures)
**Complexity:** Medium  
**Priority:** Medium  
**Conversion Effort:** 3-5 hours each

**Case-Related Operations:**
| Stored Procedure Pattern | Count | Conversion Approach |
|--------------------------|-------|---------------------|
| `usp_Case_CPT_*` | 8 | Navigation properties + Include() |
| `usp_Case_ICD_*` | 8 | Navigation properties + Include() |
| `usp_Case_FacilityType_*` | 4 | Navigation properties + Include() |
| `usp_Case_Exclusion_*` | 4 | Many-to-many relationship |
| `usp_Case_Link_*` | 4 | Self-referencing relationship |
| `usp_Case_Note_*` | 4 | One-to-many relationship |
| `usp_Case_Comment_*` | 4 | One-to-many relationship |
| `usp_Case_LetterNote_*` | 2 | One-to-many relationship |
| `usp_Case_LinkedFile_*` | 4 | One-to-many relationship |
| `usp_Case_Checklist_*` | 4 | Many-to-many through junction |

**Member-Related Operations:**
| Stored Procedure Pattern | Count | Conversion Approach |
|--------------------------|-------|---------------------|
| `usp_Member_ChronicIllness_*` | 4 | Many-to-many relationship |
| `usp_Member_Note_*` | 4 | One-to-many relationship |
| `usp_Member_MedicalAidProduct_*` | 4 | Many-to-many relationship |

**Status:** 0/56 converted (0%)

---

### Category 2: Convert to EF with Complex LINQ (Recommended: 45 procedures, 16%)

**Criteria:** Multi-table joins, complex filtering, aggregations without heavy business logic

#### 2A. Complex Search/Filter Operations (Estimated: 25 procedures)
**Complexity:** Medium-High  
**Priority:** High  
**Conversion Effort:** 4-8 hours each

| Stored Procedure | Description | EF Core Approach |
|-----------------|-------------|------------------|
| `usp_Cases_Select` | Main case search with filters | Dynamic query building with IQueryable |
| `usp_Cases_Select_ByMemberID` | Cases filtered by member | Include() + Where() |
| `usp_Cases_Select_ByRemittance` | Financial filtering | Complex joins with Include() |
| `usp_Member_Select_ByFilters` | Member search | Dynamic IQueryable with multiple filters |
| `usp_ServiceProvider_Select_ByFilters` | Provider search | Complex filtering logic |
| `usp_Bookings_Select_ByFilters` | Booking search | Date range + status filtering |
| `usp_MedicalAid_Select_ByFilters` | Medical aid search | Text search + active filter |

**Example Pattern:**
```csharp
public async Task<IEnumerable<Case>> SearchCasesAsync(CaseSearchFilters filters)
{
    var query = _context.Cases
        .Include(c => c.Member)
        .Include(c => c.ReferTo)
        .Include(c => c.ReferFrom)
        .Include(c => c.Status)
        .AsQueryable();
    
    if (!string.IsNullOrEmpty(filters.AuthNumber))
        query = query.Where(c => c.AuthNumber.Contains(filters.AuthNumber));
    
    if (filters.AdmissionDateFrom.HasValue)
        query = query.Where(c => c.AdmissionDate >= filters.AdmissionDateFrom);
    
    // ... additional filters
    
    return await query.ToListAsync();
}
```

**Status:** 3/25 converted (12%) - Member, Case, ServiceProvider search implemented

---

#### 2B. Aggregation/Reporting Queries (Estimated: 20 procedures)
**Complexity:** Medium  
**Priority:** Medium  
**Conversion Effort:** 3-6 hours each

| Stored Procedure | Description | EF Core Approach |
|-----------------|-------------|------------------|
| `usp_Case_Billing_Summary` | Case billing aggregations | GroupBy() + Sum() + Select() |
| `usp_Member_Statistics` | Member count by status | GroupBy() + Count() |
| `usp_ServiceProvider_CaseCount` | Provider workload | GroupBy() + Count() with Include() |
| `usp_Tariff_Comparison` | Tariff analysis | Complex joins + GroupBy() |

**Status:** 0/20 converted (0%)

---

### Category 3: Keep as Stored Procedure - Call via FromSqlRaw (Recommended: 52 procedures, 19%)

**Criteria:** Heavy business logic, complex calculations, performance-critical operations

#### 3A. Tariff Calculations (Recommended: 17 procedures)
**Complexity:** Very High  
**Priority:** Critical  
**Approach:** Call from EF Core using FromSqlRaw()

| Stored Procedure | Description | Reason to Keep |
|-----------------|-------------|----------------|
| `Tariff.usp_TariffCalc` | Complex tariff calculation logic | 200+ lines, nested calculations, temp tables |
| `Tariff.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate` | Date-based tariff lookup | Complex date logic with periods |
| `Tariff.usp_ServiceProvider_Tariff_Custom_Insert_FromExcel` | Bulk tariff import | Batch operations, validation logic |
| `Tariff.usp_Case_Tariff_Calculate` | Case-specific calculations | Business rules, discounts, modifiers |

**Implementation Pattern:**
```csharp
public async Task<decimal> CalculateCaseTariffAsync(int caseId, int providerId)
{
    var tariffParam = new SqlParameter("@CaseID", caseId);
    var providerParam = new SqlParameter("@ProviderID", providerId);
    var resultParam = new SqlParameter
    {
        ParameterName = "@TotalAmount",
        SqlDbType = SqlDbType.Decimal,
        Direction = ParameterDirection.Output
    };
    
    await _context.Database.ExecuteSqlRawAsync(
        "EXEC Tariff.usp_Case_Tariff_Calculate @CaseID, @ProviderID, @TotalAmount OUTPUT",
        tariffParam, providerParam, resultParam);
    
    return (decimal)resultParam.Value;
}
```

**Status:** 0/17 converted (0%) - Plan to keep as SP

---

#### 3B. Complex Business Logic (Recommended: 20 procedures)
**Complexity:** High  
**Priority:** Medium-High  
**Approach:** Keep as SP, wrap in service methods

| Stored Procedure | Description | Reason to Keep |
|-----------------|-------------|----------------|
| `usp_Case_Billing_Process` | Batch billing processing | Multi-step transaction, temp tables, cursors |
| `usp_Case_Discount_Apply` | Discount calculation & application | Complex business rules, validation logic |
| `usp_Member_ChronicIllness_Validate` | Chronic illness validation | Cross-table validation, business rules |
| `usp_Case_Link_Validate` | Case linking validation | Complex relationship validation |
| `usp_Finance_Remittance_Reconcile` | Payment reconciliation | Financial calculations, matching logic |

**Status:** 0/20 converted (0%) - Plan to keep as SP

---

#### 3C. Reporting Procedures (Recommended: 14 procedures)
**Complexity:** Medium-High  
**Priority:** Complete  
**Approach:** Replaced with EF Core queries + ClosedXML (Excel) + PuppeteerSharp (PDF)

| Stored Procedure | Description | Migration Status |
|-----------------|-------------|-----------------|
| `usp_rpt_Case_Select_BetweenDates` | Case list report | ✅ Replaced by ReportGenerationService |
| `usp_rpt_Case_Tariff_Select` | Tariff report | ✅ Still uses SP via `Tariff.usp_Case_Tariff_Select` |
| `usp_rpt_Finance_WIPExtract` | WIP financial extract | ✅ Replaced by EF Core query |
| `usp_rpt_Remittance_Summary` | Payment summary | ✅ Replaced by EF Core query |
| `usp_rpt_Case_Export` | Case data export | ✅ Replaced by EF Core query |
| `usp_rpt_Member_List` | Member list | Pending |
| `usp_rpt_Booking_Summary` | Booking report | Pending |
| `usp_rpt_ServiceProvider_Cases` | Provider workload | Pending |

**Status:** 5/8 core reports complete — no external report server needed

---

### Category 4: Keep as Stored Procedure - System Operations (Recommended: 15 procedures, 5%)

**Criteria:** Database maintenance, ETL, batch operations, system-level tasks

#### 4A. Import/ETL Operations (13 procedures)
**Complexity:** High  
**Priority:** Medium  
**Approach:** Keep as SP, expose via API endpoints

| Stored Procedure | Description | Reason to Keep |
|-----------------|-------------|----------------|
| `Import.usp_ImportDRDMemberFile` | Bulk member import | Batch operations, file parsing, validation |
| `Import.usp_ImportNAPPICodes` | NAPPI code import | External data source, bulk insert |
| `Import.usp_ImportBillingFile` | Billing file import | Complex parsing, validation, mapping |
| `Import.usp_ValidateImportData` | Import validation | Multi-table validation logic |

**Implementation:**
- Keep stored procedures for ETL logic
- Create API endpoints in Phase 11 to trigger imports
- Add file upload/validation in Angular UI

**Status:** 0/13 (0%) - Keep as SP, wrap in Phase 11

---

#### 4B. Maintenance Operations (3 procedures)
**Complexity:** N/A  
**Priority:** N/A  
**Approach:** Keep as SP, execute manually/scheduled tasks

| Stored Procedure | Description | Usage |
|-----------------|-------------|-------|
| `Maintenance.usp_FullBackup` | Database full backup | Scheduled task |
| `Maintenance.usp_LogBackup` | Transaction log backup | Scheduled task |
| `Maintenance.usp_LogBackup` | Database maintenance | Scheduled task |

**Note:** These are infrastructure procedures, not application logic. Keep as SQL Server maintenance tasks.

**Status:** N/A - System operations only

---

#### 4C. Overnight Batch Processing (2 procedures)
**Complexity:** High  
**Priority:** Low  
**Approach:** Keep as SP, trigger via scheduled job

| Stored Procedure | Description | Usage |
|-----------------|-------------|-------|
| `Overnight.usp_TariffCalc` | Nightly tariff recalculation | Scheduled batch job |
| `Overnight.usp_AllCodesData` | Code synchronization | Scheduled batch job |

**Status:** 0/2 (0%) - Keep as SP for batch processing

---

### Category 5: Deprecated/Unused (Recommended: 10 procedures, 4%)

**Approach:** Verify with business, then remove

| Stored Procedure | Last Modified | Reason |
|-----------------|---------------|--------|
| `usp_OLD_*` | < 2020 | Replaced by newer versions |
| `usp_TEMP_*` | Various | Temporary development procedures |
| `usp_Test_*` | Various | Testing procedures |

**Action:** Review with team, document removal, deprecate in Phase 12

---

## Implementation Roadmap

### Phase 3 (Current - Weeks 3-5): Core Data Access
- ✅ **Completed:** Member, Case, ServiceProvider basic CRUD (35 SPs converted)
- 🔲 **Next:** Reference data lookups (25 SPs) - 1 week
- 🔲 **Next:** Bookings, Episodes, MedicalAid CRUD (17 SPs) - 1.5 weeks

### Phase 4 (Week 5): API Layer - Reference Data
- 🔲 Controllers for all reference data entities
- 🔲 Validation and business rules
- 🔲 API documentation

### Phase 5 (Weeks 6-9): Case Management Deep Dive
- 🔲 Case-related operations (56 SPs)
  - Case_CPT (8 SPs)
  - Case_ICD (8 SPs)
  - Case_FacilityType (4 SPs)
  - Case_Exclusion (4 SPs)
  - Case_Link (4 SPs)
  - Case_Note (4 SPs)
  - Case_Comment (4 SPs)
  - Case_LetterNote (2 SPs)
  - Case_LinkedFile (4 SPs)
  - Case_Checklist (4 SPs)
  - Case_Billing (10 SPs - some kept as SP)

### Phase 6 (Weeks 9-10): Member Management
- 🔲 Member-related operations (12 SPs)
  - Member_ChronicIllness (4 SPs)
  - Member_Note (4 SPs)
  - Member_MedicalAidProduct (4 SPs)

### Phase 7 (Weeks 11-13): Finance & Billing
- 🔲 Finance operations (20 SPs)
  - Mix of EF Core + SP calls
  - Complex reconciliation logic kept as SP

### Phase 8 (Weeks 13-15): Tariff Management
- 🔲 **Keep as Stored Procedures:** All 30 tariff SPs
- 🔲 Create service wrappers using FromSqlRaw()
- 🔲 Add validation layer in C#

### Phase 10 (Weeks 17-19): Reporting Migration
- 🔲 Convert 14 report SPs to jsreport templates
- 🔲 Create jsreport server integration
- 🔲 Build Angular report viewer

### Phase 11 (Weeks 19-20): Data Import & ETL
- 🔲 Expose 13 Import SPs via API endpoints
- 🔲 Create file upload UI
- 🔲 Add import history tracking

---

## Conversion Effort Estimation

### By Category
| Category | Procedure Count | Estimated Hours | Priority |
|----------|----------------|-----------------|----------|
| 1A. Reference Data Lookups | 40 | 60 hours | High |
| 1B. Core Entity CRUD | 60 | 180 hours | High |
| 1C. Related Entity Operations | 56 | 224 hours | Medium |
| 2A. Complex Search/Filter | 25 | 150 hours | High |
| 2B. Aggregation/Reporting | 20 | 100 hours | Medium |
| 3A. Tariff Calculations (Wrap) | 17 | 34 hours | Critical |
| 3B. Complex Business Logic (Wrap) | 20 | 60 hours | High |
| 3C. Reporting (Phase 10) | 14 | 140 hours | Low |
| 4A. Import/ETL (Wrap) | 13 | 52 hours | Medium |
| 4B-C. Batch/Maintenance | 5 | N/A | N/A |
| 5. Deprecated/Remove | 10 | 10 hours | Low |

**Total Conversion Effort:** ~1,010 hours (≈ 25 weeks with 1 developer)  
**Parallelization:** With 2 developers, can reduce to 15-18 weeks

---

## Decision Matrix: Convert vs Keep

Use this flowchart to decide on conversion approach:

```
┌─────────────────────────────┐
│  Analyze Stored Procedure   │
└──────────┬──────────────────┘
           │
           ▼
    Is it system maintenance?
           │
      Yes  │  No
     ┌─────┴─────┐
     │           │
     ▼           ▼
  Keep as SP   Single table CRUD?
  (System)         │
              Yes  │  No
             ┌─────┴─────┐
             │           │
             ▼           ▼
         Convert to   Multiple joins?
         EF Core          │
         (Category 1)  Yes │  No
                      ┌────┴────┐
                      │         │
                      ▼         ▼
                   <5 joins?  Complex logic?
                      │            │
                  Yes │  No     Yes │  No
                 ┌────┴────┐   ┌────┴────┐
                 │         │   │         │
                 ▼         ▼   ▼         ▼
             Convert    Keep  Keep   Convert
             to EF    as SP  as SP   to EF
             (Cat 2)  (Cat 3)(Cat 3)(Cat 2)
```

---

## Risk Assessment

### High Risk Conversions
| Stored Procedure | Risk | Mitigation |
|-----------------|------|------------|
| Any Tariff calculation | Very High | Keep as SP, extensive testing |
| Finance reconciliation | High | Parallel testing with legacy |
| Complex case linking | Medium-High | Unit tests for all scenarios |
| Import validation logic | Medium | Test with production data samples |

### Testing Strategy
1. **Unit Tests:** All converted EF Core queries (target 80% coverage)
2. **Integration Tests:** Compare SP results vs EF Core results
3. **Performance Tests:** Benchmark EF vs SP for critical operations
4. **Acceptance Tests:** Business validation of converted logic

---

## Current Status Summary

### Converted to EF Core: 38 procedures (14%)
- ✅ Member CRUD + Search (10 SPs)
- ✅ Case CRUD + Search (12 SPs)
- ✅ ServiceProvider CRUD + Search (13 SPs)
- ✅ Reference data partial (Gender, Language, Race, Title via navigation - 3 SPs equivalent)

### Planned for Conversion: 156 procedures (56%)
- 🔲 Reference data lookups (37 remaining)
- 🔲 Core entity CRUD (25 remaining)
- 🔲 Related entity operations (56)
- 🔲 Complex queries (38)

### Keep as Stored Procedure: 77 procedures (28%)
- 🔲 Tariff calculations (17) - Wrap in service methods
- 🔲 Complex business logic (20) - Wrap in service methods
- 🔲 Reporting (14) - Migrate to jsreport Phase 10
- 🔲 Import/ETL (13) - Expose via API Phase 11
- 🔲 Batch/Maintenance (7) - System operations
- 🔲 Overnight processing (2) - Scheduled jobs

### Deprecated/Review: 10 procedures (4%)
- 🔲 OLD_*, TEMP_*, Test_* procedures

---

## Next Actions

### Immediate (This Week)
1. ✅ **Complete:** Unit of Work pattern implementation
2. 🔲 **Next:** Convert remaining reference data SPs (MarritalStatus, MemberStatus, etc.)
3. 🔲 **Next:** Create repository interfaces for Bookings, Episodes
4. 🔲 **Next:** Write unit tests for converted repositories

### Next Sprint (Week 6)
1. 🔲 Implement Bookings service layer
2. 🔲 Implement Episode service layer
3. 🔲 Implement MedicalAid service layer
4. 🔲 Begin Case-related operations (Case_CPT, Case_ICD)

### Documentation Needed
1. 🔲 SP → EF Core conversion guide (with examples)
2. 🔲 Performance comparison report (SP vs EF)
3. 🔲 FromSqlRaw() usage patterns and best practices
4. 🔲 Testing strategy for converted procedures

---

## Appendix A: Stored Procedure Inventory

### Full List by Schema

#### dbo Schema (245 procedures)
```
Reference Data (40 SPs):
- BillingStatus (4), CaseStatus (4), CaseType (4), ChecklistTemplate (4)
- ChronicIllness (4), Country (4), FacilityType (4), Gender (4)
- Language (4), MarritalStatus (4), MemberStatus (4), PeriodInCountry (4)
- Race (4), Speciality (6), Title (4)

Core Entities (95 SPs):
- Case operations (50), Member operations (10), Cases operations (12)
- ServiceProvider (13), MedicalAid (12), Episode (7), Bookings (6)

Related Operations (96 SPs):
- Case_CPT (8), Case_ICD (8), Case_FacilityType (4), Case_Exclusion (4)
- Case_Link (4), Case_Note (4), Case_Comment (4), Case_LetterNote (2)
- Case_LinkedFile (4), Case_Checklist (4), Case_Billing (10)
- Member_ChronicIllness (4), Member_Note (4), Member_MedicalAidProduct (4)
- ServiceProvider_Tariff (8), MedicalAid_Exclusion (4), etc.

Reporting (14 SPs):
- rpt_Case_* (7), rpt_Finance_* (4), rpt_Remittance_* (2), rpt_Member_* (1)
```

#### Tariff Schema (13 procedures)
```
- Tariff calculation (5)
- ServiceProvider_Tariff management (4)
- Case_Tariff operations (4)
```

#### Import Schema (13 procedures)
```
- ImportDRDMemberFile, ImportNAPPICodes, ImportBillingFile
- Validation and mapping procedures (10)
```

#### Maintenance Schema (3 procedures)
```
- FullBackup, LogBackup, DatabaseMaintenance
```

#### Updates Schema (2 procedures)
```
- AppUpdates_Select, AppUpdates_Insert
```

#### Overnight Schema (2 procedures)
```
- TariffCalc, AllCodesData
```

---

## Appendix B: Example Conversions

### Example 1: Simple Reference Data
**Before (SQL):**
```sql
CREATE PROCEDURE usp_Gender_Select
AS
BEGIN
    SELECT GenderID, Gender, DateInserted, UserID
    FROM shared.Gender
    WHERE DateDeleted IS NULL
    ORDER BY Gender
END
```

**After (EF Core):**
```csharp
public async Task<IEnumerable<Gender>> GetAllAsync()
{
    return await _context.Genders
        .Where(g => g.DateDeleted == null)
        .OrderBy(g => g.Gender1)
        .ToListAsync();
}
```

### Example 2: Complex Search
**Before (SQL):**
```sql
CREATE PROCEDURE usp_Cases_Select_ByFilters
    @MemberID INT = NULL,
    @StatusID INT = NULL,
    @DateFrom DATETIME = NULL,
    @DateTo DATETIME = NULL
AS
BEGIN
    SELECT c.*, m.MemberNumber, sp.ServiceProviderName
    FROM dbo.Cases c
    INNER JOIN shared.Member m ON c.MemberID = m.MemberID
    LEFT JOIN shared.ServiceProvider sp ON c.ReferToID = sp.ServiceProviderID
    WHERE c.DateDeleted IS NULL
      AND (@MemberID IS NULL OR c.MemberID = @MemberID)
      AND (@StatusID IS NULL OR c.StatusID = @StatusID)
      AND (@DateFrom IS NULL OR c.AdmissionDate >= @DateFrom)
      AND (@DateTo IS NULL OR c.AdmissionDate <= @DateTo)
    ORDER BY c.AdmissionDate DESC
END
```

**After (EF Core):**
```csharp
public async Task<IEnumerable<Case>> SearchAsync(CaseSearchFilters filters)
{
    var query = _context.Cases
        .Include(c => c.Member)
        .Include(c => c.ReferTo)
        .Where(c => c.DateDeleted == null);
    
    if (filters.MemberId.HasValue)
        query = query.Where(c => c.MemberId == filters.MemberId);
    
    if (filters.StatusId.HasValue)
        query = query.Where(c => c.StatusId == filters.StatusId);
    
    if (filters.AdmissionDateFrom.HasValue)
        query = query.Where(c => c.AdmissionDate >= filters.AdmissionDateFrom);
    
    if (filters.AdmissionDateTo.HasValue)
        query = query.Where(c => c.AdmissionDate <= filters.AdmissionDateTo);
    
    return await query
        .OrderByDescending(c => c.AdmissionDate)
        .ToListAsync();
}
```

### Example 3: Kept as SP with FromSqlRaw
**Before (SQL):**
```sql
CREATE PROCEDURE Tariff.usp_Case_Tariff_Calculate
    @CaseID INT,
    @ProviderID INT,
    @TotalAmount DECIMAL(18,2) OUTPUT
AS
BEGIN
    -- Complex tariff calculation logic (200+ lines)
    -- Multiple temp tables, cursors, nested calculations
    -- ... (kept as stored procedure)
END
```

**After (Service Wrapper):**
```csharp
public async Task<decimal> CalculateCaseTariffAsync(int caseId, int providerId)
{
    var caseParam = new SqlParameter("@CaseID", caseId);
    var providerParam = new SqlParameter("@ProviderID", providerId);
    var amountParam = new SqlParameter
    {
        ParameterName = "@TotalAmount",
        SqlDbType = SqlDbType.Decimal,
        Precision = 18,
        Scale = 2,
        Direction = ParameterDirection.Output
    };
    
    await _context.Database.ExecuteSqlRawAsync(
        "EXEC Tariff.usp_Case_Tariff_Calculate @CaseID, @ProviderID, @TotalAmount OUTPUT",
        caseParam, providerParam, amountParam);
    
    return (decimal)amountParam.Value;
}
```

---

**Document Version:** 1.0  
**Last Updated:** April 22, 2026  
**Next Review:** After Phase 5 completion (Week 9)
