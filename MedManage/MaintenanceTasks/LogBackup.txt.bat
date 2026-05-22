SET DBNAME=MEDMANAGE_TEST

GOTO :s_time

:s_time
  :: Get the time
  For /f "tokens=1-3 delims=1234567890 " %%a in ("%time%") Do Set "delims=%%a%%b%%c"
  For /f "tokens=1-4 delims=%delims%" %%G in ("%time%") Do (
    Set _hh=%%G
    Set _min=%%H
    Set _ss=%%I
    Set _ms=%%J
  )
  :: Strip any leading spaces
  Set _hh=%_hh: =%

  :: Ensure the hours have a leading zero
  if 1%_hh% LSS 20 Set _hh=0%_hh%

SET DEST=E:\DATA\%DBNAME%_%date:~10,4%%date:~7,2%%date:~4,2%%_hh%%_min%%_ss%.LOG.BAK
ECHO %DEST%

sqlcmd -S . -d MedManage_Test -Q "EXEC [Maintenance].[usp_LogBackup] '%DBNAME%','%DEST%'"

del %DEST%