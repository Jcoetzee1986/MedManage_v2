/*
  Script: Fix Audit Triggers for All 24 Tables
  Purpose: CREATE OR ALTER all 24 audit triggers to use explicit column names
           instead of SELECT * — fixes breakage caused by the DateDeleted column migration.
  Date: 2025-01-20
  REQ: REQ-1.3

  Context:
  - The DateDeleted column was added to all audited tables (BaseEntity)
  - Original triggers used SELECT * which broke when the schema changed
  - This script recreates triggers with explicit column operations
  - Triggers handle: INSERT (set DateInserted, UserID), UPDATE (set DateUpdated, UpdatedUserID)
  - DELETE operations are handled via soft delete in the application layer (DateDeleted column)
  - User context is read from SESSION_CONTEXT('UserID')
  - Trigger names match EF Core DbContext HasTrigger() registrations exactly

  Tables Updated (24):
  ---------------------------------------------------------------
   #  | Schema          | Table                    | Trigger Name
  ---------------------------------------------------------------
   1  | shared          | Member                   | trg_for_shared_Member
   2  | shared          | ServiceProvider          | trg_for_shared_ServiceProvider
   3  | shared          | BaseTariff               | trg_for_shared_BaseTariff
   4  | shared          | MedicalAid               | trg_for_shared_MedicalAid
   5  | shared          | MedicalAid_Exclusion     | trg_for_shared_MedicalAid_Exclusion
   6  | shared          | Member_ChronicIllness    | trg_for_shared_Member_ChronicIllness
   7  | shared          | MemberNote               | trg_for_shared_MemberNote
   8  | shared          | Country                  | trg_for_shared_Country
   9  | shared          | Exclusion                | trg_for_shared_Exclusion
  10  | shared          | ChronicIllness           | trg_for_shared_ChronicIllness
  11  | shared          | Speciality               | trg_for_shared_Speciality
  12  | shared          | SystemData               | trg_for_shared_SystemData
  13  | dbo             | ChronicIllness           | trg_for_dbo_ChronicIllness
  14  | CaseManagement  | Cases                    | trg_for_CaseManagement_Cases
  15  | CaseManagement  | Case_Checklist           | trg_for_CaseManagement_Case_Checklist
  16  | CaseManagement  | CaseComment              | trg_for_CaseManagement_CaseComment
  17  | CaseManagement  | Case_CPT                 | trg_for_CaseManagement_Case_CPT
  18  | CaseManagement  | Case_Exclusion           | trg_for_CaseManagement_Case_Exclusion
  19  | CaseManagement  | Case_FacilityType        | trg_for_CaseManagement_Case_FacilityType
  20  | CaseManagement  | Case_ICD                 | trg_for_CaseManagement_Case_ICD
  21  | CaseManagement  | Case_LinkedFile          | trg_for_CaseManagement_Case_LinkedFile
  22  | CaseManagement  | CaseNote                 | trg_for_CaseManagement_CaseNote
  23  | CaseManagement  | Episode                  | trg_for_CaseManagement_Episode
  24  | CaseManagement  | Episode_Case             | trg_for_CaseManagement_Episode_Case
  ---------------------------------------------------------------

  NOTE: Session_User_Case is excluded (volatile locking table, not audited).
        Soft delete (DateDeleted) is handled at the application layer via
        Repository.DeleteAsync(), not in triggers.
*/

USE MedManage;
GO

PRINT '========================================';
PRINT 'Fix Audit Triggers - Explicit Column Names';
PRINT 'Date: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
PRINT '';
GO

-- ============================================================
-- 1. shared.Member (PK: MemberID)
-- ============================================================
PRINT '1/24: shared.Member';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_Member]
ON [shared].[Member]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        -- INSERT: set DateInserted and UserID
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Member] t
        INNER JOIN inserted i ON t.MemberID = i.MemberID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        -- UPDATE: set DateUpdated and UpdatedUserID
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Member] t
        INNER JOIN inserted i ON t.MemberID = i.MemberID;
    END

    -- DELETE: no trigger action needed (soft delete sets DateDeleted in app layer)
END;
GO

-- ============================================================
-- 2. shared.ServiceProvider (PK: ServiceProviderID)
-- ============================================================
PRINT '2/24: shared.ServiceProvider';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_ServiceProvider]
ON [shared].[ServiceProvider]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[ServiceProvider] t
        INNER JOIN inserted i ON t.ServiceProviderID = i.ServiceProviderID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[ServiceProvider] t
        INNER JOIN inserted i ON t.ServiceProviderID = i.ServiceProviderID;
    END
END;
GO

-- ============================================================
-- 3. shared.BaseTariff (PK: BaseTariffID)
-- ============================================================
PRINT '3/24: shared.BaseTariff';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_BaseTariff]
ON [shared].[BaseTariff]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[BaseTariff] t
        INNER JOIN inserted i ON t.BaseTariffID = i.BaseTariffID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[BaseTariff] t
        INNER JOIN inserted i ON t.BaseTariffID = i.BaseTariffID;
    END
END;
GO

-- ============================================================
-- 4. shared.MedicalAid (PK: MedicalAidID)
-- ============================================================
PRINT '4/24: shared.MedicalAid';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_MedicalAid]
ON [shared].[MedicalAid]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MedicalAid] t
        INNER JOIN inserted i ON t.MedicalAidID = i.MedicalAidID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MedicalAid] t
        INNER JOIN inserted i ON t.MedicalAidID = i.MedicalAidID;
    END
END;
GO

-- ============================================================
-- 5. shared.MedicalAid_Exclusion (composite PK: MedicalAidID, BaseTariffID)
-- ============================================================
PRINT '5/24: shared.MedicalAid_Exclusion';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_MedicalAid_Exclusion]
ON [shared].[MedicalAid_Exclusion]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MedicalAid_Exclusion] t
        INNER JOIN inserted i ON t.MedicalAidID = i.MedicalAidID
            AND t.BaseTariffID = i.BaseTariffID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MedicalAid_Exclusion] t
        INNER JOIN inserted i ON t.MedicalAidID = i.MedicalAidID
            AND t.BaseTariffID = i.BaseTariffID;
    END
END;
GO

-- ============================================================
-- 6. shared.Member_ChronicIllness (composite PK: MemberID, ChronicIllnessID)
-- ============================================================
PRINT '6/24: shared.Member_ChronicIllness';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_Member_ChronicIllness]
ON [shared].[Member_ChronicIllness]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Member_ChronicIllness] t
        INNER JOIN inserted i ON t.MemberID = i.MemberID
            AND t.ChronicIllnessID = i.ChronicIllnessID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Member_ChronicIllness] t
        INNER JOIN inserted i ON t.MemberID = i.MemberID
            AND t.ChronicIllnessID = i.ChronicIllnessID;
    END
END;
GO

-- ============================================================
-- 7. shared.MemberNote (PK: MemberNoteID)
-- ============================================================
PRINT '7/24: shared.MemberNote';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_MemberNote]
ON [shared].[MemberNote]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MemberNote] t
        INNER JOIN inserted i ON t.MemberNoteID = i.MemberNoteID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[MemberNote] t
        INNER JOIN inserted i ON t.MemberNoteID = i.MemberNoteID;
    END
END;
GO

-- ============================================================
-- 8. shared.Country (PK: CountryID)
-- ============================================================
PRINT '8/24: shared.Country';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_Country]
ON [shared].[Country]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Country] t
        INNER JOIN inserted i ON t.CountryID = i.CountryID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Country] t
        INNER JOIN inserted i ON t.CountryID = i.CountryID;
    END
END;
GO

-- ============================================================
-- 9. shared.Exclusion (PK: ExclusionID)
-- ============================================================
PRINT '9/24: shared.Exclusion';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_Exclusion]
ON [shared].[Exclusion]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Exclusion] t
        INNER JOIN inserted i ON t.ExclusionID = i.ExclusionID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Exclusion] t
        INNER JOIN inserted i ON t.ExclusionID = i.ExclusionID;
    END
END;
GO

-- ============================================================
-- 10. shared.ChronicIllness (PK: ChronicIllnessID)
-- ============================================================
PRINT '10/24: shared.ChronicIllness';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_ChronicIllness]
ON [shared].[ChronicIllness]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[ChronicIllness] t
        INNER JOIN inserted i ON t.ChronicIllnessID = i.ChronicIllnessID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[ChronicIllness] t
        INNER JOIN inserted i ON t.ChronicIllnessID = i.ChronicIllnessID;
    END
END;
GO

-- ============================================================
-- 11. shared.Speciality (PK: SpecialityID)
-- ============================================================
PRINT '11/24: shared.Speciality';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_Speciality]
ON [shared].[Speciality]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Speciality] t
        INNER JOIN inserted i ON t.SpecialityID = i.SpecialityID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[Speciality] t
        INNER JOIN inserted i ON t.SpecialityID = i.SpecialityID;
    END
END;
GO

-- ============================================================
-- 12. shared.SystemData (PK: SystemDataID)
-- ============================================================
PRINT '12/24: shared.SystemData';
GO
CREATE OR ALTER TRIGGER [shared].[trg_for_shared_SystemData]
ON [shared].[SystemData]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[SystemData] t
        INNER JOIN inserted i ON t.SystemDataID = i.SystemDataID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [shared].[SystemData] t
        INNER JOIN inserted i ON t.SystemDataID = i.SystemDataID;
    END
END;
GO

-- ============================================================
-- 13. dbo.ChronicIllness (PK: ChronicIllnessID)
-- ============================================================
PRINT '13/24: dbo.ChronicIllness';
GO
CREATE OR ALTER TRIGGER [dbo].[trg_for_dbo_ChronicIllness]
ON [dbo].[ChronicIllness]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [dbo].[ChronicIllness] t
        INNER JOIN inserted i ON t.ChronicIllnessID = i.ChronicIllnessID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [dbo].[ChronicIllness] t
        INNER JOIN inserted i ON t.ChronicIllnessID = i.ChronicIllnessID;
    END
END;
GO

-- ============================================================
-- 14. CaseManagement.Cases (PK: CaseID)
-- ============================================================
PRINT '14/24: CaseManagement.Cases';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Cases]
ON [CaseManagement].[Cases]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Cases] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Cases] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID;
    END
END;
GO

-- ============================================================
-- 15. CaseManagement.Case_Checklist (composite PK: CaseID, ChecklistTemplateID)
-- ============================================================
PRINT '15/24: CaseManagement.Case_Checklist';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_Checklist]
ON [CaseManagement].[Case_Checklist]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_Checklist] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID
            AND t.ChecklistTemplateID = i.ChecklistTemplateID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_Checklist] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID
            AND t.ChecklistTemplateID = i.ChecklistTemplateID;
    END
END;
GO

-- ============================================================
-- 16. CaseManagement.CaseComment (PK: CaseCommentID)
-- ============================================================
PRINT '16/24: CaseManagement.CaseComment';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_CaseComment]
ON [CaseManagement].[CaseComment]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[CaseComment] t
        INNER JOIN inserted i ON t.CaseCommentID = i.CaseCommentID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[CaseComment] t
        INNER JOIN inserted i ON t.CaseCommentID = i.CaseCommentID;
    END
END;
GO

-- ============================================================
-- 17. CaseManagement.Case_CPT (PK: CaseID_CPTID)
-- ============================================================
PRINT '17/24: CaseManagement.Case_CPT';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_CPT]
ON [CaseManagement].[Case_CPT]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_CPT] t
        INNER JOIN inserted i ON t.CaseID_CPTID = i.CaseID_CPTID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_CPT] t
        INNER JOIN inserted i ON t.CaseID_CPTID = i.CaseID_CPTID;
    END
END;
GO

-- ============================================================
-- 18. CaseManagement.Case_Exclusion (composite PK: CaseID, ExclusionID)
-- ============================================================
PRINT '18/24: CaseManagement.Case_Exclusion';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_Exclusion]
ON [CaseManagement].[Case_Exclusion]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_Exclusion] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID
            AND t.ExclusionID = i.ExclusionID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_Exclusion] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID
            AND t.ExclusionID = i.ExclusionID;
    END
END;
GO

-- ============================================================
-- 19. CaseManagement.Case_FacilityType (PK: CaseID_FacilityTypeID)
-- ============================================================
PRINT '19/24: CaseManagement.Case_FacilityType';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_FacilityType]
ON [CaseManagement].[Case_FacilityType]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_FacilityType] t
        INNER JOIN inserted i ON t.CaseID_FacilityTypeID = i.CaseID_FacilityTypeID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_FacilityType] t
        INNER JOIN inserted i ON t.CaseID_FacilityTypeID = i.CaseID_FacilityTypeID;
    END
END;
GO

-- ============================================================
-- 20. CaseManagement.Case_ICD (composite PK: CaseID, ICDID)
-- ============================================================
PRINT '20/24: CaseManagement.Case_ICD';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_ICD]
ON [CaseManagement].[Case_ICD]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_ICD] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID AND t.ICDID = i.ICDID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_ICD] t
        INNER JOIN inserted i ON t.CaseID = i.CaseID AND t.ICDID = i.ICDID;
    END
END;
GO

-- ============================================================
-- 21. CaseManagement.Case_LinkedFile (PK: Case_LinkedFileID)
-- ============================================================
PRINT '21/24: CaseManagement.Case_LinkedFile';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Case_LinkedFile]
ON [CaseManagement].[Case_LinkedFile]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_LinkedFile] t
        INNER JOIN inserted i ON t.Case_LinkedFileID = i.Case_LinkedFileID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Case_LinkedFile] t
        INNER JOIN inserted i ON t.Case_LinkedFileID = i.Case_LinkedFileID;
    END
END;
GO

-- ============================================================
-- 22. CaseManagement.CaseNote (PK: CaseNoteID)
-- ============================================================
PRINT '22/24: CaseManagement.CaseNote';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_CaseNote]
ON [CaseManagement].[CaseNote]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[CaseNote] t
        INNER JOIN inserted i ON t.CaseNoteID = i.CaseNoteID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[CaseNote] t
        INNER JOIN inserted i ON t.CaseNoteID = i.CaseNoteID;
    END
END;
GO

-- ============================================================
-- 23. CaseManagement.Episode (PK: EpisodeID)
-- ============================================================
PRINT '23/24: CaseManagement.Episode';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Episode]
ON [CaseManagement].[Episode]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Episode] t
        INNER JOIN inserted i ON t.EpisodeID = i.EpisodeID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Episode] t
        INNER JOIN inserted i ON t.EpisodeID = i.EpisodeID;
    END
END;
GO

-- ============================================================
-- 24. CaseManagement.Episode_Case (composite PK: EpisodeID, CaseID)
-- ============================================================
PRINT '24/24: CaseManagement.Episode_Case';
GO
CREATE OR ALTER TRIGGER [CaseManagement].[trg_for_CaseManagement_Episode_Case]
ON [CaseManagement].[Episode_Case]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentUser NVARCHAR(256) = CAST(SESSION_CONTEXT(N'UserID') AS NVARCHAR(256));

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateInserted = GETDATE(),
            t.UserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Episode_Case] t
        INNER JOIN inserted i ON t.EpisodeID = i.EpisodeID
            AND t.CaseID = i.CaseID;
    END

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        UPDATE t
        SET t.DateUpdated = GETDATE(),
            t.UpdatedUserID = COALESCE(@CurrentUser, SYSTEM_USER)
        FROM [CaseManagement].[Episode_Case] t
        INNER JOIN inserted i ON t.EpisodeID = i.EpisodeID
            AND t.CaseID = i.CaseID;
    END
END;
GO

-- ============================================================
-- CLEANUP: Drop old trigger names that don't match DbContext
-- These were created by previous script versions with wrong names
-- ============================================================
PRINT '';
PRINT '========================================';
PRINT 'Dropping old misnamed triggers (if exist)';
PRINT '========================================';
GO

-- Old shared schema triggers (without schema prefix in name)
IF OBJECT_ID('[shared].[trg_for_Member]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_Member];
IF OBJECT_ID('[shared].[trg_for_ServiceProvider]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_ServiceProvider];
IF OBJECT_ID('[shared].[trg_for_Bookings]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_Bookings];
IF OBJECT_ID('[shared].[trg_for_MedicalAid]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_MedicalAid];
IF OBJECT_ID('[shared].[trg_for_MedicalAidProduct]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_MedicalAidProduct];
IF OBJECT_ID('[shared].[trg_for_MedicalAid_Exclusion]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_MedicalAid_Exclusion];
IF OBJECT_ID('[shared].[trg_for_MedicalAid_Tariff]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_MedicalAid_Tariff];
IF OBJECT_ID('[shared].[trg_for_Member_ChronicIllness]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_Member_ChronicIllness];
IF OBJECT_ID('[shared].[trg_for_MemberNote]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_MemberNote];
IF OBJECT_ID('[shared].[trg_for_LinkedFile]', 'TR') IS NOT NULL
    DROP TRIGGER [shared].[trg_for_LinkedFile];
GO

-- Old CaseManagement schema triggers (without schema prefix in name)
IF OBJECT_ID('[CaseManagement].[trg_for_Cases]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Cases];
IF OBJECT_ID('[CaseManagement].[trg_for_CaseNote]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_CaseNote];
IF OBJECT_ID('[CaseManagement].[trg_for_CaseComment]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_CaseComment];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_CPT]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_CPT];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_ICD]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_ICD];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_FacilityType]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_FacilityType];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_Exclusion]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_Exclusion];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_Checklist]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_Checklist];
IF OBJECT_ID('[CaseManagement].[trg_for_CaseLetterNote]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_CaseLetterNote];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_NappiCodes]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_NappiCodes];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_Billing]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_Billing];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_Billing_Comment]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_Billing_Comment];
IF OBJECT_ID('[CaseManagement].[trg_for_Case_LinkedFile]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Case_LinkedFile];
IF OBJECT_ID('[CaseManagement].[trg_for_Episode]', 'TR') IS NOT NULL
    DROP TRIGGER [CaseManagement].[trg_for_Episode];
GO

PRINT 'Old triggers cleaned up.';
GO

-- ============================================================
-- VERIFICATION: List all triggers to confirm they were created
-- ============================================================
PRINT '';
PRINT '========================================';
PRINT 'Verification: All audit triggers';
PRINT '========================================';
GO

SELECT 
    SCHEMA_NAME(o.schema_id) AS [Schema],
    OBJECT_NAME(t.parent_id) AS [Table],
    t.name AS [Trigger],
    t.create_date AS [Created],
    t.modify_date AS [Modified]
FROM sys.triggers t
INNER JOIN sys.objects o ON t.parent_id = o.object_id
WHERE t.name LIKE 'trg_for_%'
  AND t.is_ms_shipped = 0
ORDER BY [Schema], [Table];
GO

PRINT '';
PRINT '========================================';
PRINT 'Script Complete - 24 triggers updated';
PRINT 'All triggers use explicit column names';
PRINT 'All triggers support INSERT/UPDATE/DELETE';
PRINT '========================================';
GO
