-- Create PasswordResetTokens Table
-- This table stores password reset tokens and PINs for user password recovery

CREATE TABLE [dbo].[PasswordResetTokens]
(
    -- Primary Key
    [TokenId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    
    -- Foreign Key to aspnet_Users
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    
    -- Unique token for URL-based reset (future use)
    [Token] NVARCHAR(100) NOT NULL,
    
    -- 6-digit PIN for email-based reset
    [Pin] NVARCHAR(6) NOT NULL,
    
    -- Email address (denormalized for quick lookup)
    [Email] NVARCHAR(256) NOT NULL,
    
    -- Token expiration (typically 1 hour from creation)
    [ExpiresAt] DATETIME NOT NULL,
    
    -- Flag to track if token has been used
    [IsUsed] BIT NOT NULL DEFAULT 0,
    
    -- Timestamp when token was created
    [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    
    -- Audit fields (inherited from BaseEntity pattern)
    [DateInserted] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedByUserID] INT NULL,
    [DateUpdated] DATETIME NULL,
    [UpdatedByUserID] INT NULL,
    
    -- Foreign Key Constraint
    CONSTRAINT [FK_PasswordResetTokens_AspnetUsers] 
        FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[aspnet_Users]([UserId])
        ON DELETE CASCADE
);

-- Create Indexes
-- Unique index on Token for URL-based lookups
CREATE UNIQUE NONCLUSTERED INDEX [IX_PasswordResetTokens_Token] 
    ON [dbo].[PasswordResetTokens]([Token]);

-- Unique index on Pin for PIN-based lookups
CREATE UNIQUE NONCLUSTERED INDEX [IX_PasswordResetTokens_Pin] 
    ON [dbo].[PasswordResetTokens]([Pin]);

-- Index on UserId for user-based queries
CREATE NONCLUSTERED INDEX [IX_PasswordResetTokens_UserId] 
    ON [dbo].[PasswordResetTokens]([UserId]);

-- Composite index for email + PIN lookups (common query pattern)
CREATE NONCLUSTERED INDEX [IX_PasswordResetTokens_Email_Pin] 
    ON [dbo].[PasswordResetTokens]([Email], [Pin])
    INCLUDE ([ExpiresAt], [IsUsed]);

GO

-- Sample query to verify table creation
SELECT 
    OBJECT_NAME(object_id) AS TableName,
    name AS IndexName,
    type_desc AS IndexType,
    is_unique AS IsUnique
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.PasswordResetTokens')
ORDER BY index_id;
