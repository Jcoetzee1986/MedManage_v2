/*
Add Audit Columns Script
This script adds DateUpdated and UpdatedUserID columns to all tables that don't already have them.
Run this against the MedManage database.
*/

USE MedManage
GO

-- Generate ALTER TABLE statements for all base tables that are missing audit columns
DECLARE @sql NVARCHAR(MAX) = '';

-- Add DateUpdated column where it doesn't exist
SELECT @sql = @sql + 
    'ALTER TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + 
    ' ADD DateUpdated DATETIME NULL;' + CHAR(13) + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
  AND NOT EXISTS (
      SELECT 1 
      FROM INFORMATION_SCHEMA.COLUMNS c 
      WHERE c.TABLE_SCHEMA = t.TABLE_SCHEMA 
        AND c.TABLE_NAME = t.TABLE_NAME 
        AND c.COLUMN_NAME = 'DateUpdated'
  )
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Add UpdatedUserID column where it doesn't exist
SELECT @sql = @sql + 
    'ALTER TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + 
    ' ADD UpdatedUserID NVARCHAR(450) NULL;' + CHAR(13) + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
  AND NOT EXISTS (
      SELECT 1 
      FROM INFORMATION_SCHEMA.COLUMNS c 
      WHERE c.TABLE_SCHEMA = t.TABLE_SCHEMA 
        AND c.TABLE_NAME = t.TABLE_NAME 
        AND c.COLUMN_NAME = 'UpdatedUserID'
  )
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Add DateInserted where it doesn't exist (default to GETDATE())
SELECT @sql = @sql + 
    'ALTER TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + 
    ' ADD DateInserted DATETIME NOT NULL CONSTRAINT DF_' + REPLACE(TABLE_NAME, ' ', '_') + '_DateInserted DEFAULT GETDATE();' + CHAR(13) + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
  AND NOT EXISTS (
      SELECT 1 
      FROM INFORMATION_SCHEMA.COLUMNS c 
      WHERE c.TABLE_SCHEMA = t.TABLE_SCHEMA 
        AND c.TABLE_NAME = t.TABLE_NAME 
        AND c.COLUMN_NAME = 'DateInserted'
  )
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Add UserID where it doesn't exist (but check for existing UserId with different case)
SELECT @sql = @sql + 
    'ALTER TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + 
    ' ADD UserID NVARCHAR(450) NULL;' + CHAR(13) + CHAR(10)
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
  AND NOT EXISTS (
      SELECT 1 
      FROM INFORMATION_SCHEMA.COLUMNS c 
      WHERE c.TABLE_SCHEMA = t.TABLE_SCHEMA 
        AND c.TABLE_NAME = t.TABLE_NAME 
        AND LOWER(c.COLUMN_NAME) = 'userid'
  )
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Print the script for review
PRINT '-- Generated Audit Column Script:'
PRINT @sql;

-- UNCOMMENT THE LINE BELOW TO EXECUTE THE SCRIPT
-- EXEC sp_executesql @sql;
GO

-- After running the above, you can create indexes on the audit columns for better performance:
/*
-- Create indexes on audit columns (optional, for better query performance)
CREATE NONCLUSTERED INDEX IX_DateInserted ON [schema].[TableName] (DateInserted);
CREATE NONCLUSTERED INDEX IX_DateUpdated ON [schema].[TableName] (DateUpdated);
*/
