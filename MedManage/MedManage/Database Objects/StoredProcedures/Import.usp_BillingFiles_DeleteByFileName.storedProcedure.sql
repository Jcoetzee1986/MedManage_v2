USE [MedManage]
GO
/****** Object:  StoredProcedure [Import].[usp_BillingFilesPracticeNameMatch_Insert]    Script Date: 2016/04/25 11:29:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--TRUNCATE TABLE [Import].[BillingFiles]
--UPDATE [Import].[BillingFiles]
--SET PRNumber = LTRIM(RTRIM(REPLACE(REPLACE(PRNumber,':',''),';','')))

ALTER PROCEDURE [Import].[usp_BillingFiles_DeleteByFileName]
@FileName Varchar(500)
AS

BEGIN

DELETE FROM [Import].[BillingFiles]
WHERE [FileName] = @FileName

END

