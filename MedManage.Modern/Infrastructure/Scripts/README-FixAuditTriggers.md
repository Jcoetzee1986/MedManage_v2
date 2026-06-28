# Fix Audit Triggers — Documentation

## Purpose

Fixes all 24 audit triggers that broke when the `DateDeleted` column was added to tables
as part of the soft-delete migration. The original triggers used `SELECT *` which caused
column count mismatches after schema changes.

## What Changed

Each trigger was rewritten to:
1. **Use explicit column operations** — only sets `DateInserted`, `UserID`, `DateUpdated`, `UpdatedUserID`
2. **Support INSERT, UPDATE, and DELETE** — trigger fires on all DML operations
3. **Use correct trigger names** — names match EF Core `HasTrigger()` registrations exactly
4. **Read user context from SESSION_CONTEXT** — enables the app layer to pass the authenticated user

## Trigger Behavior

| Operation | Action |
|-----------|--------|
| INSERT | Sets `DateInserted = GETDATE()`, `UserID = SESSION_CONTEXT('UserID')` |
| UPDATE | Sets `DateUpdated = GETDATE()`, `UpdatedUserID = SESSION_CONTEXT('UserID')` |
| DELETE | No trigger action (soft delete handled in app layer via `DateDeleted`) |

## Tables Updated (24)

| # | Schema | Table | Trigger Name | PK Column(s) |
|---|--------|-------|--------------|--------------|
| 1 | shared | Member | trg_for_shared_Member | MemberID |
| 2 | shared | ServiceProvider | trg_for_shared_ServiceProvider | ServiceProviderID |
| 3 | shared | BaseTariff | trg_for_shared_BaseTariff | BaseTariffID |
| 4 | shared | MedicalAid | trg_for_shared_MedicalAid | MedicalAidID |
| 5 | shared | MedicalAid_Exclusion | trg_for_shared_MedicalAid_Exclusion | MedicalAidID, BaseTariffID |
| 6 | shared | Member_ChronicIllness | trg_for_shared_Member_ChronicIllness | MemberID, ChronicIllnessID |
| 7 | shared | MemberNote | trg_for_shared_MemberNote | MemberNoteID |
| 8 | shared | Country | trg_for_shared_Country | CountryID |
| 9 | shared | Exclusion | trg_for_shared_Exclusion | ExclusionID |
| 10 | shared | ChronicIllness | trg_for_shared_ChronicIllness | ChronicIllnessID |
| 11 | shared | Speciality | trg_for_shared_Speciality | SpecialityID |
| 12 | shared | SystemData | trg_for_shared_SystemData | SystemDataID |
| 13 | dbo | ChronicIllness | trg_for_dbo_ChronicIllness | ChronicIllnessID |
| 14 | CaseManagement | Cases | trg_for_CaseManagement_Cases | CaseID |
| 15 | CaseManagement | Case_Checklist | trg_for_CaseManagement_Case_Checklist | CaseID, ChecklistTemplateID |
| 16 | CaseManagement | CaseComment | trg_for_CaseManagement_CaseComment | CaseCommentID |
| 17 | CaseManagement | Case_CPT | trg_for_CaseManagement_Case_CPT | CaseID_CPTID |
| 18 | CaseManagement | Case_Exclusion | trg_for_CaseManagement_Case_Exclusion | CaseID, ExclusionID |
| 19 | CaseManagement | Case_FacilityType | trg_for_CaseManagement_Case_FacilityType | CaseID_FacilityTypeID |
| 20 | CaseManagement | Case_ICD | trg_for_CaseManagement_Case_ICD | CaseID, ICDID |
| 21 | CaseManagement | Case_LinkedFile | trg_for_CaseManagement_Case_LinkedFile | Case_LinkedFileID |
| 22 | CaseManagement | CaseNote | trg_for_CaseManagement_CaseNote | CaseNoteID |
| 23 | CaseManagement | Episode | trg_for_CaseManagement_Episode | EpisodeID |
| 24 | CaseManagement | Episode_Case | trg_for_CaseManagement_Episode_Case | EpisodeID, CaseID |

## Excluded Tables

- **Session_User_Case** — volatile session locking table, not audited business data

## Old Triggers Cleaned Up

The script drops old trigger names from a previous version that used incorrect naming:
- `trg_for_Member` → replaced by `trg_for_shared_Member`
- `trg_for_Cases` → replaced by `trg_for_CaseManagement_Cases`
- etc.

## How to Run

```sql
-- Connect to the MedManage database and execute:
sqlcmd -S <server> -d MedManage -i Infrastructure\Scripts\002_FixAuditTriggers.sql
```

Or open in SSMS and execute the full script.

## Prerequisites

- SQL Server 2016+ (for SESSION_CONTEXT support)
- `db_owner` or `ALTER` permission on the target schemas
- Application must set SESSION_CONTEXT('UserID') before DML operations

## Related

- **REQ-1.3** in requirements.md
- **BaseEntity.cs** — defines DateInserted, UserID, DateUpdated, UpdatedUserID, DateDeleted
- **MedManageDbContext.Configuration.cs** — registers trigger names via HasTrigger()
- **ICurrentUserService** — sets SESSION_CONTEXT in the connection (see task: Wire up ICurrentUserService)
