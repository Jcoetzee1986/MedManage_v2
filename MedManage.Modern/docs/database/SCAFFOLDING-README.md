# Database Entity Scaffolding - MedManage

## Summary

Successfully scaffolded **101 entity classes** and **93 database tables** from the MedManage database.

## What Was Done

### 1. Upgraded to .NET 10.0
- All projects upgraded from .NET 9.0 to .NET 10.0
- Microsoft packages updated to version 10.0.0
- Swashbuckle.AspNetCore set to version 6.9.0 for compatibility

### 2. Created Base Entity Classes
Updated `BaseEntity.cs` and `TenantEntity.cs` with standard audit columns:
- `DateInserted` (DateTime) - Auto-set on insert
- `UserID` (string) - User who created the record
- `DateUpdated` (DateTime?) - Auto-set on update
- `UpdatedUserID` (string?) - User who last updated the record

### 3. Scaffolded Database Entities
Generated 101 entity files in `src/MedManage.Infrastructure/Entities/` including:
- Case management entities (Cases, CaseBilling, CaseNotes, etc.)
- Member & medical aid entities
- Service provider entities
- Lookup/reference data entities
- Views (prefixed with `Vw`)

### 4. Configured DbContext
- Created `MedManageDbContext` with all DbSets
- Added `SaveChangesAsync` override to automatically populate audit columns
- Split configuration into partial class (`MedManageDbContext.Configuration.cs`)
- Updated connection string to use correct database

## Generated Files

```
src/MedManage.Infrastructure/
├── Entities/              (101 entity .cs files)
├── Persistence/
│   ├── MedManageDbContext.cs                    (Main DbContext)
│   ├── MedManage DbContext.Configuration.cs      (EF configurations)
│   └── UnitOfWork.cs
├── Repositories/
docs/database/
└── Add-AuditColumns.sql   (Script to add missing audit columns)
```

## Next Steps

### 1. Add Missing Audit Columns to Database

Run the SQL script to add `DateUpdated` and `UpdatedUserID` columns to all tables:

```sql
-- Location: docs/database/Add-AuditColumns.sql
-- This script will:
-- 1. Generate ALTER TABLE statements for all tables missing audit columns
-- 2. Print the script for review
-- 3. Execute only when you uncomment the final line
```

**To execute:**
1. Open SQL Server Management Studio or Azure Data Studio
2. Connect to your SQL Server instance
3. Open `docs/database/Add-AuditColumns.sql`
4. Review the generated script
5. Uncomment the `EXEC sp_executesql @sql;` line
6. Execute the script

### 2. Re-scaffold Entities After Database Changes

Once you've added the audit columns to the database, re-scaffold the entities:

```powershell
cd src/MedManage.Infrastructure
dotnet ef dbcontext scaffold "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true" `
  Microsoft.EntityFrameworkCore.SqlServer `
  --output-dir Entities `
  --context-dir Persistence `
  --context MedManageDbContextTemp `
  --force `
  --data-annotations `
  --no-onconfiguring
```

Then manually merge the updated entity properties into your existing entities.

### 3. Implement User Context Service

Create a service to populate `UserID` and `UpdatedUserID` from the current user:

```csharp
// TODO: Create ICurrentUserService interface
// TODO: Inject into DbContext constructor
// TODO: Update SaveChangesAsync to use current user
```

### 4. Update Entity Base Classes

Some entities may need to inherit from `BaseEntity` or `TenantEntity` to get automatic audit column population.

## Entities Overview

### Main Entity Categories:

**Case Management:**
- Case, CaseBilling, CaseComment, CaseNote, CaseTariff
- Case_CPT, Case_ICD, Case_Exclusion, Case_FacilityType
- Episode, EpisodeCase

**Member Management:**
- Member, MemberChronicIllness, MemberNote, MemberStatus
- MemberMedicalAidProduct

**Medical Aid:**
- MedicalAid, MedicalAidProduct, MedicalAidExclusion
- MainClient, MainClientTariff, MainClientMedicalAidProduct

**Service Providers:**
- ServiceProvider, ServiceProviderTariff, ServiceProviderTariffCustom
- ServiceProviderMainClientDiscount

**Reference Data:**
- BaseTariff, Tariff, TariffName, TariffCalc
- CPT, ICD, NappiCode
- Country, Gender, Language, Race, Title, Speciality
- ChronicIllness, Exclusion, FacilityType

**System:**
- SystemData, AppUpdate, ClientUpdate
- SessionUserCase

**Views:**  
23 views for reporting and queries (prefixed with `Vw`)

## Configuration Notes

### Connection String
Updated in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true"
}
```

### Audit Column Auto-Population
The `SaveChangesAsync` method in `MedManageDbContext` automatically sets:
- `DateInserted` and `UserID` on new entities
- `DateUpdated` and `UpdatedUserID` on modified entities

### Multi-Tenancy Support
`TenantEntity` base class includes `MainClientID` for tenant isolation (currently commented out in DbContext).

## Warnings

You may see these warnings during build/restore:
```
Package 'System.Security.Cryptography.Xml' 10.0.0 has known high severity vulnerabilities
```
This is a transitive dependency from Entity Framework Core. Microsoft needs to release a patch to fully resolve this issue.

## Troubleshooting

### If scaffolding fails:
1. Ensure .NET 10.0 SDK is installed: `dotnet --version`
2. Ensure EF Core tools are installed: `dotnet ef --version`
3. Verify database connectivity: Test connection string in SSMS
4. Check that database has tables: `SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'`

### If build fails:
1. Clean and rebuild: `dotnet clean && dotnet build`
2. Check for missing using statements in entity files
3. Verify all partial class files are in the same namespace

## Resources

- [EF Core Database First](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/)
- [EF Core Command Line Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
