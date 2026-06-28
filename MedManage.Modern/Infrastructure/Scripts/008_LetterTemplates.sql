-- Create LetterTemplate table for storing HTML-based report templates
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[shared].[LetterTemplate]'))
BEGIN
    CREATE TABLE [shared].[LetterTemplate] (
        [LetterTemplateID] INT IDENTITY(1,1) PRIMARY KEY,
        [MainClientID] INT NULL,
        [TemplateName] NVARCHAR(100) NOT NULL,
        [TemplateType] NVARCHAR(50) NOT NULL DEFAULT 'CaseLetter',
        [HtmlContent] NVARCHAR(MAX) NULL,
        [HeaderHtml] NVARCHAR(MAX) NULL,
        [FooterHtml] NVARCHAR(MAX) NULL,
        [LogoBase64] NVARCHAR(MAX) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [IsDefault] BIT NOT NULL DEFAULT 0,
        [DateInserted] DATETIME NULL DEFAULT GETDATE(),
        [UserID] NVARCHAR(450) NULL,
        [DateUpdated] DATETIME NULL,
        [UpdatedUserID] NVARCHAR(450) NULL,
        [DateDeleted] DATETIME NULL
    );

    PRINT 'Created shared.LetterTemplate table';
END
GO
