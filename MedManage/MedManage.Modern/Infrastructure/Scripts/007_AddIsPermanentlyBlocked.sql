/*
  Script: Add IsPermanentlyBlocked column to aspnet_Membership
  Purpose: Supports permanent account deactivation for resigned staff.
           Blocked users remain in the database for audit tracking but cannot log in.
  Date: 2026-06-26
*/

USE MedManage;
GO

PRINT '========================================';
PRINT 'Adding IsPermanentlyBlocked column';
PRINT 'Date: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
PRINT '';

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.aspnet_Membership') 
    AND name = 'IsPermanentlyBlocked'
)
BEGIN
    ALTER TABLE dbo.aspnet_Membership 
    ADD IsPermanentlyBlocked BIT NOT NULL DEFAULT 0;
    
    PRINT '✓ Column IsPermanentlyBlocked added to aspnet_Membership';
END
ELSE
BEGIN
    PRINT '→ Column IsPermanentlyBlocked already exists (skipped)';
END
GO

PRINT '';
PRINT 'Script Complete';
GO
