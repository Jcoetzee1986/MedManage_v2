-- Add DateDeleted column to all tables that don't have it yet
-- This script adds the soft delete column for implementing the soft delete pattern

DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql = @sql + 
    'IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N''' + 
    QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ''') AND name = ''DateDeleted'')
BEGIN
    ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' 
    ADD DateDeleted datetime NULL;
    PRINT ''Added DateDeleted to ' + s.name + '.' + t.name + ''';
END
'
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE t.type = 'U' -- User tables only
  AND t.name NOT LIKE'__EF%' -- Skip EF migrations history
  AND t.name NOT LIKE 'aspnet_%' -- Skip ASP.NET tables
  AND s.name NOT IN ('sys', 'INFORMATION_SCHEMA') -- Skip system schemas
ORDER BY s.name, t.name;

PRINT @sql;
EXEC sp_executesql @sql;

PRINT 'DateDeleted column added to all applicable tables';
