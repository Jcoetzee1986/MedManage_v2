-- =============================================
-- Script: Standardize DateInserted Columns
-- Description: Changes DateInserted from DATE to DATETIME for consistency with BaseEntity
-- Author: System
-- Date: 2026-04-18
-- =============================================

USE [MedManage]
GO

PRINT 'Starting DateInserted column standardization...'
GO

-- =============================================
-- Case_CPT
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[CaseManagement].[Case_CPT]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating Case_CPT.DateInserted from DATE to DATETIME...'
    
    -- Drop default constraint if exists
    DECLARE @ConstraintName1 NVARCHAR(200)
    SELECT @ConstraintName1 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[CaseManagement].[Case_CPT]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName1 IS NOT NULL
        EXEC('ALTER TABLE [CaseManagement].[Case_CPT] DROP CONSTRAINT ' + @ConstraintName1)
    
    -- Alter column type
    ALTER TABLE [CaseManagement].[Case_CPT] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'Case_CPT.DateInserted updated successfully.'
END
ELSE
    PRINT 'Case_CPT.DateInserted already DATETIME or does not exist.'
GO

-- =============================================
-- Case_ICD
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[CaseManagement].[Case_ICD]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating Case_ICD.DateInserted from DATE to DATETIME...'
    
    DECLARE @ConstraintName2 NVARCHAR(200)
    SELECT @ConstraintName2 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[CaseManagement].[Case_ICD]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName2 IS NOT NULL
        EXEC('ALTER TABLE [CaseManagement].[Case_ICD] DROP CONSTRAINT ' + @ConstraintName2)
    
    ALTER TABLE [CaseManagement].[Case_ICD] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'Case_ICD.DateInserted updated successfully.'
END
ELSE
    PRINT 'Case_ICD.DateInserted already DATETIME or does not exist.'
GO

-- =============================================
-- Case_Exclusion
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[CaseManagement].[Case_Exclusion]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating Case_Exclusion.DateInserted from DATE to DATETIME...'
    
    DECLARE @ConstraintName3 NVARCHAR(200)
    SELECT @ConstraintName3 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[CaseManagement].[Case_Exclusion]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName3 IS NOT NULL
        EXEC('ALTER TABLE [CaseManagement].[Case_Exclusion] DROP CONSTRAINT ' + @ConstraintName3)
    
    ALTER TABLE [CaseManagement].[Case_Exclusion] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'Case_Exclusion.DateInserted updated successfully.'
END
ELSE
    PRINT 'Case_Exclusion.DateInserted already DATETIME or does not exist.'
GO

-- =============================================
-- Case_FacilityType
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[CaseManagement].[Case_FacilityType]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating Case_FacilityType.DateInserted from DATE to DATETIME...'
    
    DECLARE @ConstraintName4 NVARCHAR(200)
    SELECT @ConstraintName4 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[CaseManagement].[Case_FacilityType]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName4 IS NOT NULL
        EXEC('ALTER TABLE [CaseManagement].[Case_FacilityType] DROP CONSTRAINT ' + @ConstraintName4)
    
    ALTER TABLE [CaseManagement].[Case_FacilityType] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'Case_FacilityType.DateInserted updated successfully.'
END
ELSE
    PRINT 'Case_FacilityType.DateInserted already DATETIME or does not exist.'
GO

-- =============================================
-- Case_Tariff
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[Tariff].[Case_Tariff]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating Case_Tariff.DateInserted from DATE to DATETIME...'
    
    DECLARE @ConstraintName5 NVARCHAR(200)
    SELECT @ConstraintName5 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[Tariff].[Case_Tariff]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName5 IS NOT NULL
        EXEC('ALTER TABLE [Tariff].[Case_Tariff] DROP CONSTRAINT ' + @ConstraintName5)
    
    ALTER TABLE [Tariff].[Case_Tariff] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'Case_Tariff.DateInserted updated successfully.'
END
ELSE
    PRINT 'Case_Tariff.DateInserted already DATETIME or does not exist.'
GO

-- =============================================
-- TariffCalc (Overnight schema)
-- =============================================
IF EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('[Overnight].[TariffCalc]') 
    AND name = 'DateInserted' 
    AND system_type_id = TYPE_ID('date')
)
BEGIN
    PRINT 'Updating TariffCalc.DateInserted from DATE to DATETIME...'
    
    DECLARE @ConstraintName6 NVARCHAR(200)
    SELECT @ConstraintName6 = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE c.object_id = OBJECT_ID('[Overnight].[TariffCalc]')
    AND c.name = 'DateInserted'
    
    IF @ConstraintName6 IS NOT NULL
        EXEC('ALTER TABLE [Overnight].[TariffCalc] DROP CONSTRAINT ' + @ConstraintName6)
    
    ALTER TABLE [Overnight].[TariffCalc] 
    ALTER COLUMN [DateInserted] DATETIME NULL
    
    PRINT 'TariffCalc.DateInserted updated successfully.'
END
ELSE
    PRINT 'TariffCalc.DateInserted already DATETIME or does not exist.'
GO

PRINT ''
PRINT '============================================='
PRINT 'DateInserted column standardization complete!'
PRINT '============================================='
PRINT ''
PRINT 'Summary:'
PRINT '- All DateInserted columns are now DATETIME type'
PRINT '- Entities can now inherit from BaseEntity'
PRINT ''
PRINT 'Next Steps:'
PRINT '1. Update entity classes to inherit from BaseEntity'
PRINT '2. Remove duplicate DateInserted, UserID, DateUpdated, UpdatedUserID properties'
PRINT '3. Optionally re-scaffold entities to verify'
GO
