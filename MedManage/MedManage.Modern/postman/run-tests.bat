@echo off
REM MedManage API Test Runner - Windows Batch Version
REM Quick test execution script for Windows Command Prompt

SETLOCAL EnableDelayedExpansion

echo ========================================
echo MedManage API Test Runner
echo ========================================
echo.

REM Check if Newman is installed
where newman >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Newman is not installed globally.
    echo.
    echo Install Newman with: npm install -g newman
    echo Or run with npx: npx newman run MedManage.postman_collection.json -e MedManage.postman_environment.json --insecure
    echo.
    pause
    exit /b 1
)

REM Get Newman version
for /f "tokens=*" %%i in ('newman --version 2^>^&1') do set NEWMAN_VERSION=%%i
echo Newman installed: %NEWMAN_VERSION%
echo.

REM Create reports directory if it doesn't exist
if not exist "reports" mkdir reports

REM Run tests
echo Running tests...
echo.

newman run MedManage.postman_collection.json ^
  -e MedManage.postman_environment.json ^
  --insecure ^
  -r cli

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo ^✓ ALL TESTS PASSED
    echo ========================================
) else (
    echo.
    echo ========================================
    echo ^✗ TESTS FAILED
    echo ========================================
    exit /b %ERRORLEVEL%
)

ENDLOCAL
