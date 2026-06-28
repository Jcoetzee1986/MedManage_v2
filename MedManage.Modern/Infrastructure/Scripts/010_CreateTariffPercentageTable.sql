-- ============================================================================
-- Script: 010_CreateTariffPercentageTable.sql
-- Description: Creates the [Tariff].[TariffPercentage] table for managing
--              tariff percentage records used in service provider tariff
--              propagation and case letter generation.
-- ============================================================================

-- Ensure the Tariff schema exists
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'Tariff')
BEGIN
    EXEC(N'CREATE SCHEMA [Tariff]');
END
GO

-- Create the TariffPercentage table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tariff].[TariffPercentage]') AND type = N'U')
BEGIN
    CREATE TABLE [Tariff].[TariffPercentage]
    (
        [TariffPercentageId]    INT             NOT NULL IDENTITY(1, 1),
        [PercentageAdded]       DECIMAL(10, 4)  NOT NULL,
        [TariffPeriodName]      INT             NOT NULL,
        [StartActiveDate]       DATE            NOT NULL,
        [EndActiveDate]         DATE            NULL,
        [Status]                NVARCHAR(50)    NULL,
        [RecordsAffected]       INT             NULL,
        [Notes]                 NVARCHAR(500)   NULL,
        [DateInserted]          DATETIME        NULL,
        [UserID]                NVARCHAR(MAX)   NULL,
        [DateUpdated]           DATETIME        NULL,
        [UpdatedUserID]         NVARCHAR(MAX)   NULL,
        [DateDeleted]           DATETIME        NULL,

        CONSTRAINT [PK_TariffPercentage] PRIMARY KEY CLUSTERED ([TariffPercentageId] ASC)
    );
END
GO

-- Index for propagation queries (closing prior period records)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_TariffPercentage_TariffPeriodName_EndActiveDate' AND object_id = OBJECT_ID(N'[Tariff].[TariffPercentage]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_TariffPercentage_TariffPeriodName_EndActiveDate]
        ON [Tariff].[TariffPercentage] ([TariffPeriodName], [EndActiveDate]);
END
GO

-- Index for overlap detection on create/update
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_TariffPercentage_TariffPeriodName_StartActiveDate' AND object_id = OBJECT_ID(N'[Tariff].[TariffPercentage]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_TariffPercentage_TariffPeriodName_StartActiveDate]
        ON [Tariff].[TariffPercentage] ([TariffPeriodName], [StartActiveDate]);
END
GO

-- ============================================================================
-- Seed data: initial tariff percentages
-- ============================================================================

-- 233.90% for 2025
IF NOT EXISTS (SELECT 1 FROM [Tariff].[TariffPercentage] WHERE [TariffPeriodName] = 2025 AND [PercentageAdded] = 233.9000 AND [DateDeleted] IS NULL)
BEGIN
    INSERT INTO [Tariff].[TariffPercentage]
        ([PercentageAdded], [TariffPeriodName], [StartActiveDate], [EndActiveDate], [Status], [RecordsAffected], [Notes], [DateInserted], [UserID])
    VALUES
        (233.9000, 2025, '2025-01-01', '2025-12-31', N'Completed', 0, N'Initial seed data', GETDATE(), N'SYSTEM');
END
GO

-- 250.60% for 2026
IF NOT EXISTS (SELECT 1 FROM [Tariff].[TariffPercentage] WHERE [TariffPeriodName] = 2026 AND [PercentageAdded] = 250.6000 AND [DateDeleted] IS NULL)
BEGIN
    INSERT INTO [Tariff].[TariffPercentage]
        ([PercentageAdded], [TariffPeriodName], [StartActiveDate], [EndActiveDate], [Status], [RecordsAffected], [Notes], [DateInserted], [UserID])
    VALUES
        (250.6000, 2026, '2026-01-01', NULL, N'Completed', 0, N'Initial seed data', GETDATE(), N'SYSTEM');
END
GO
