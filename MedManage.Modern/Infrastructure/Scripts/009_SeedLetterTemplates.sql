-- Seed letter templates for each main client
-- The HTML templates use Handlebars syntax: {{FieldName}}, {{#if}}, {{#each}}
-- Logo is pulled from MainClientLogo via the report SP data

DECLARE @defaultHtml NVARCHAR(MAX) = (SELECT HtmlContent FROM shared.LetterTemplate WHERE IsDefault = 1 AND TemplateType = 'CaseLetter')

-- Only insert if templates don't already exist for these clients
IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 1 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (1, 'Botswana to SA - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 2 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (2, 'DRD - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 3 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (3, 'Medline Africa - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 4 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (4, 'Gold One - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 5 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (5, 'Swaziland - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 6 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (6, 'Botswana Local - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 7 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (7, 'Nigeria - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 8 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (8, 'MedHealth - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 9 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (9, 'Bayport - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 10 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (10, 'MOH Zambia - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 11 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (11, 'Malawi - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 12 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (12, 'Mozambique - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

IF NOT EXISTS (SELECT 1 FROM shared.LetterTemplate WHERE MainClientID = 13 AND TemplateType = 'CaseLetter')
    INSERT INTO shared.LetterTemplate (MainClientID, TemplateName, TemplateType, HtmlContent, IsActive, IsDefault, DateInserted)
    VALUES (13, 'UNI-Health Mozambique - Case Letter', 'CaseLetter', @defaultHtml, 1, 0, GETDATE())

PRINT 'Seeded ' + CAST(@@ROWCOUNT AS VARCHAR) + ' client-specific letter templates'
GO
