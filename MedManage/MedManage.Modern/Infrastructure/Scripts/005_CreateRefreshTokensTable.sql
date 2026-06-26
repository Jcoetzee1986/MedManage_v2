-- =============================================
-- Script: Create RefreshTokens Table
-- Description: Creates the RefreshTokens table for JWT token rotation
-- Author: System
-- Date: 2026-04-18
-- =============================================

USE [MedManage]
GO

-- Drop table if exists (for development only - comment out in production)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[RefreshTokens]
    PRINT 'Dropped existing RefreshTokens table'
END
GO

-- Create RefreshTokens table
CREATE TABLE [dbo].[RefreshTokens](
    [TokenId] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
    [UserId] [uniqueidentifier] NOT NULL,
    [Token] [nvarchar](500) NOT NULL,
    [ExpiresAt] [datetime] NOT NULL,
    [CreatedDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
    [RevokedAt] [datetime] NULL,
    [IsUsed] [bit] NOT NULL DEFAULT 0,
    [IpAddress] [nvarchar](45) NULL,
    [UserAgent] [nvarchar](500) NULL,
    [ReplacedByTokenId] [uniqueidentifier] NULL,
    [RevocationReason] [nvarchar](200) NULL,
    
    -- BaseEntity audit columns (renamed to avoid conflict)
    [DateInserted] [datetime] NOT NULL DEFAULT GETUTCDATE(),
    [CreatedByUserID] [nvarchar](256) NOT NULL DEFAULT '',
    [DateUpdated] [datetime] NULL,
    [UpdatedByUserID] [nvarchar](256) NULL,
    
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([TokenId] ASC)
)
GO

-- Create unique index on Token
CREATE UNIQUE NONCLUSTERED INDEX [IX_RefreshTokens_Token] 
ON [dbo].[RefreshTokens] ([Token] ASC)
GO

-- Create index on UserId for faster lookup
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] 
ON [dbo].[RefreshTokens] ([UserId] ASC)
GO

-- Create index on ExpiresAt for cleanup queries
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_ExpiresAt] 
ON [dbo].[RefreshTokens] ([ExpiresAt] ASC)
GO

-- Create foreign key to aspnet_Users
ALTER TABLE [dbo].[RefreshTokens]  
WITH CHECK ADD CONSTRAINT [FK_RefreshTokens_aspnet_Users] 
FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RefreshTokens] 
CHECK CONSTRAINT [FK_RefreshTokens_aspnet_Users]
GO

PRINT 'RefreshTokens table created successfully'
GO

-- Grant permissions (adjust as needed for your security model)
--GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[RefreshTokens] TO [public]
--GO

--PRINT 'Permissions granted'
GO
