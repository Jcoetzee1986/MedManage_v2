/*
  Script: Remove All Audit Triggers
  Purpose: Drop all audit triggers that were broken by DateDeleted migration
  Date: 2026-04-22
  
  Context: 
  - Migration added DateDeleted to audit tables (13 columns)
  - Triggers use positional INSERT with only 7 values
  - This breaks all INSERT/UPDATE operations on tables with triggers
  - Auditing will be handled in application code instead
*/

USE MedManage;
GO

-- Log trigger removal
DECLARE @TriggerCount INT = 0;
DECLARE @TriggerName NVARCHAR(256);
DECLARE @SchemaName NVARCHAR(128);
DECLARE @TableName NVARCHAR(256);
DECLARE @SQL NVARCHAR(MAX);

PRINT '========================================';
PRINT 'Removing Audit Triggers';
PRINT 'Date: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
PRINT '';

-- Create temporary table to hold triggers to drop
DECLARE @TriggersToRemove TABLE (
    TriggerName NVARCHAR(256),
    SchemaName NVARCHAR(128),
    TableName NVARCHAR(256)
);

-- Find all audit triggers (trg_for_* pattern)
INSERT INTO @TriggersToRemove (TriggerName, SchemaName, TableName)
SELECT 
    t.name AS TriggerName,
    SCHEMA_NAME(o.schema_id) AS SchemaName,
    OBJECT_NAME(t.parent_id) AS TableName
FROM sys.triggers t
INNER JOIN sys.objects o ON t.parent_id = o.object_id
WHERE t.name LIKE 'trg_for_%'
  AND t.is_ms_shipped = 0  -- Exclude system triggers
ORDER BY SchemaName, TableName, TriggerName;

-- Display triggers to be removed
SELECT @TriggerCount = COUNT(*) FROM @TriggersToRemove;

PRINT 'Found ' + CAST(@TriggerCount AS VARCHAR) + ' audit triggers to remove:';
PRINT '';

SELECT 
    SchemaName + '.' + TableName AS [Table],
    TriggerName AS [Trigger]
FROM @TriggersToRemove
ORDER BY SchemaName, TableName;

PRINT '';
PRINT '========================================';
PRINT 'Dropping Triggers...';
PRINT '========================================';
PRINT '';

-- Cursor to drop each trigger
DECLARE trigger_cursor CURSOR FOR
SELECT TriggerName, SchemaName, TableName
FROM @TriggersToRemove;

OPEN trigger_cursor;

FETCH NEXT FROM trigger_cursor INTO @TriggerName, @SchemaName, @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRY
        -- Build DROP statement
        SET @SQL = 'DROP TRIGGER IF EXISTS ' + QUOTENAME(@SchemaName) + '.' + QUOTENAME(@TriggerName);
        
        -- Execute
        EXEC sp_executesql @SQL;
        
        PRINT '✓ Dropped: ' + @SchemaName + '.' + @TriggerName + ' (on ' + @SchemaName + '.' + @TableName + ')';
        
    END TRY
    BEGIN CATCH
        PRINT '✗ Failed to drop: ' + @SchemaName + '.' + @TriggerName;
        PRINT '  Error: ' + ERROR_MESSAGE();
    END CATCH
    
    FETCH NEXT FROM trigger_cursor INTO @TriggerName, @SchemaName, @TableName;
END

CLOSE trigger_cursor;
DEALLOCATE trigger_cursor;

PRINT '';
PRINT '========================================';
PRINT 'Verification';
PRINT '========================================';
PRINT '';

-- Verify all triggers removed
DECLARE @RemainingCount INT;

SELECT @RemainingCount = COUNT(*)
FROM sys.triggers t
WHERE t.name LIKE 'trg_for_%'
  AND t.is_ms_shipped = 0;

IF @RemainingCount = 0
BEGIN
    PRINT '✓ SUCCESS: All ' + CAST(@TriggerCount AS VARCHAR) + ' audit triggers removed';
END
ELSE
BEGIN
    PRINT '⚠ WARNING: ' + CAST(@RemainingCount AS VARCHAR) + ' audit triggers still exist:';
    PRINT '';
    
    SELECT 
        SCHEMA_NAME(o.schema_id) + '.' + OBJECT_NAME(t.parent_id) AS [Table],
        t.name AS [Trigger]
    FROM sys.triggers t
    INNER JOIN sys.objects o ON t.parent_id = o.object_id
    WHERE t.name LIKE 'trg_for_%'
      AND t.is_ms_shipped = 0;
END

PRINT '';
PRINT '========================================';
PRINT 'Script Complete';
PRINT '========================================';

GO
