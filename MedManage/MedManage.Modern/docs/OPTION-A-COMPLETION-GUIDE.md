# Option A Implementation - Completion Guide

## ✅ What's Been Completed

1. **BaseEntity Updated** - Added `DateTime? DateDeleted` property ([BaseEntity.cs](src/MedManage.Core/Entities/BaseEntity.cs#L23))
2. **Build Errors Reduced** - From 120 errors down to ~20 (83% reduction)
3. **Major Property Fixes** - PracticeNr, Remittance, Title1, BillingStatus1, Speciality1
4. **Navigation Properties Fixed** - Cpt, Icd, removed non-existent includes

## 🔧 Remaining Issues (~20 errors)

### Quick Reference Table

| Issue | Location | Current Code | Should Be | Priority |
|-------|----------|--------------|-----------|----------|
| Case.Status navigation | CaseRepository.cs | `c.CaseStatus` | `c.Status` | High |
| Race property | RaceRepository.cs | `RaceDescription` | `Race1` (verify entity) | Medium |
| ChecklistTemplate | ChecklistTemplateRepository.cs | `ChecklistTemplate1` | Verify entity property | Medium |
| NappiCode properties | NappiCodeRepository.cs | `NappiDescription`, `Date` | Verify entity schema | Medium |
| Episode navigation | EpisodeRepository.cs | `EpisodeCases` | Remove include or verify | Low |
| Case navigation | CaseRepository.cs (multiple) | `CaseBillings` | Remove include or verify | Low |
| CaseLink property | CaseLinkRepository.cs | `CaseId` | Verify entity schema | Low |

## 🚀 Step-by-Step Completion

### Step 1: Fix Case.Status Navigation (~6 locations)

**Find:** `c.CaseStatus` or `x.CaseStatus`  
**Replace with:** `c.Status` or `x.Status`

**Files to Update:**
- CaseRepository.cs (multiple locations in SearchByFiltersAsync)

### Step 2: Verify and Fix Reference Data Properties

Check actual entity property names:

```powershell
# Check Race entity
Get-Content "src\MedManage.Core\Entities\Race.cs"

# Check ChecklistTemplate entity
Get-Content "src\MedManage.Core\Entities\ChecklistTemplate.cs"

# Check NappiCode entity
Get-Content "src\MedManage.Core\Entities\NappiCode.cs"
```

Then update repositories accordingly (likely `Race1`, `ChecklistTemplate1` pattern).

### Step 3: Remove Non-Existent Navigation Properties

**EpisodeRepository.cs** - If `EpisodeCases` doesn't exist:
```csharp
// Remove this:
.Include(e => e.EpisodeCases)
    .ThenInclude(ec => ec.Case)
        .ThenInclude(c => c.Member)

// Keep this:
.Where(e => e.EpisodeId == episodeId && e.DateDeleted == null)
```

**CaseRepository.cs** - If `CaseBillings` doesn't exist:
```csharp
// Remove this:
.Include(c => c.CaseBillings)

// Keep filtering by ID instead
```

### Step 4: Fix Case.ServiceProviderId Issue

Case entity uses `ReferToId` and `ReferFromId`, not `ServiceProviderId`.

**CaseRepository.cs** - CopyCaseAsync method:
```csharp
// Change this:
newCase.ServiceProviderId = sourceCase.ServiceProviderId;
newCase.CaseTypeId = sourceCase.CaseTypeId;
newCase.CaseStatusId = sourceCase.CaseStatusId;

// To this (if these properties don't exist, remove them):
newCase.ReferToId = sourceCase.ReferToId;
newCase.ReferFromId = sourceCase.ReferFromId;
newCase.StatusId = sourceCase.StatusId;
```

### Step 5: Create Database Migration for DateDeleted

**Option A: Manual SQL Script** (Quick):
```sql
-- Add DateDeleted to all tables that inherit from BaseEntity
-- Run this on your MedManage database

-- Example for a few key tables:
ALTER TABLE CaseManagement.Cases ADD DateDeleted datetime NULL;
ALTER TABLE shared.Member ADD DateDeleted datetime NULL;
ALTER TABLE shared.ServiceProvider ADD DateDeleted datetime NULL;
ALTER TABLE shared.Title ADD DateDeleted datetime NULL;
-- ... repeat for all tables
```

**Option B: EF Core Migration** (Proper way):
```powershell
# After fixing all build errors:
dotnet build MedManage.Modern.sln

# Generate migration:
dotnet ef migrations add AddSoftDeleteSupport `
  --project src/MedManage.Infrastructure `
  --startup-project src/MedManage.API

# Apply to database:
dotnet ef database update `
  --project src/MedManage.Infrastructure `
  --startup-project src/MedManage.API
```

### Step 6: Build and Verify

```powershell
cd MedManage.Modern
dotnet clean
dotnet build MedManage.Modern.sln
```

**Target: ZERO errors, ZERO warnings**

## 📝 Quick Fix Script

If you want to batch-fix the most common issues:

```powershell
# Run from MedManage.Modern root
$repoPath = "src\MedManage.Infrastructure\Repositories"

# Fix Case.Status references
Get-ChildItem "$repoPath\CaseRepository.cs" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $content = $content -replace '\.CaseStatus', '.Status'
    $content | Set-Content $_.FullName -NoNewline
}

# Fix remaining property names (adjust based on actual entity properties)
Get-ChildItem "$repoPath\*.cs" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $content = $content -replace 'RaceDescription', 'Race1'
    $content = $content -replace '\.EpisodeCases\)', ')'  # Remove entire Include
    $content = $content -replace '\.CaseBillings\)', ')'  # Remove entire Include
    $content | Set-Content $_.FullName -NoNewline
}
```

## ✅ Success Criteria

Before moving to Phase 4:

- [ ] Solution builds with **zero errors**
- [ ] DateDeleted property exists on BaseEntity
- [ ] DateDeleted columns added to database (via migration or SQL)
- [ ] All 49 repositories registered in DI container
- [ ] Sample API call works (e.g., GET /api/countries)

## 📞 Next Steps After Completion

1. Test repository DI registration:
   ```csharp
   // In any controller:
   private readonly ICountryRepository _countryRepo;
   public TestController(ICountryRepository countryRepo) => _countryRepo = countryRepo;
   
   [HttpGet("test")]
   public async Task<IActionResult> Test() => Ok(await _countryRepo.GetActiveAsync());
   ```

2. Move to Phase 4: Service Layer & Business Logic
3. Create DTOs and AutoMapper profiles
4. Build out API controllers for core entities

## 🐛 Troubleshooting

**If build still fails after fixes:**

1. Check actual entity property names:
   ```powershell
   # Search all entities for a property name
   Get-ChildItem -Path "src\MedManage.Core\Entities\*.cs" -Recurse | 
     Select-String "public.*Description"
   ```

2. Compare with repository usage:
   ```powershell
   # Find all uses of a property in repositories
   Get-ChildItem -Path "src\MedManage.Infrastructure\Repositories\*.cs" | 
     Select-String "RaceDescription"
   ```

3. Use Visual Studio's error list to jump directly to issues

4. If navigation property truly doesn't exist, remove the Include() statement entirely

---

**Estimated Time to Complete:** 30-60 minutes  
**Difficulty:** Medium (mostly find/replace operations)  
**Impact:** Enables Phase 4 development with fully functional repository layer
