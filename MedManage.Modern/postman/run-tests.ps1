#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Run MedManage API Postman tests using Newman CLI

.DESCRIPTION
    Executes the Postman collection test suite for MedManage API.
    Automatically creates reports directory and generates HTML reports.

.PARAMETER Environment
    Environment to run tests against (local, staging, production)
    Default: local

.PARAMETER Folder
    Specific folder/test suite to run
    Options: "Member CRUD Operations", "Validation Tests", "Performance Tests"
    Default: All tests

.PARAMETER Report
    Generate HTML report
    Default: $true

.PARAMETER Verbose
    Show verbose Newman output
    Default: $false

.PARAMETER SkipSSL
    Skip SSL certificate verification (for self-signed certs)
    Default: $true (development environment)

.EXAMPLE
    .\run-tests.ps1
    Run all tests with default settings

.EXAMPLE
    .\run-tests.ps1 -Verbose
    Run all tests with verbose output

.EXAMPLE
    .\run-tests.ps1 -Folder "Member CRUD Operations"
    Run only CRUD operation tests

.EXAMPLE
    .\run-tests.ps1 -Environment staging -SkipSSL:$false
    Run tests against staging environment with SSL verification
#>

param(
    [ValidateSet('local', 'staging', 'production')]
    [string]$Environment = 'local',
    
    [string]$Folder = '',
    
    [bool]$Report = $true,
    
    [switch]$Verbose,
    
    [bool]$SkipSSL = $true
)

# Set error action preference
$ErrorActionPreference = 'Stop'

# Colors for output
$ColorSuccess = 'Green'
$ColorError = 'Red'
$ColorWarning = 'Yellow'
$ColorInfo = 'Cyan'

Write-Host "========================================" -ForegroundColor $ColorInfo
Write-Host "MedManage API Test Runner" -ForegroundColor $ColorInfo
Write-Host "========================================" -ForegroundColor $ColorInfo
Write-Host ""

# Check if Newman is installed
Write-Host "Checking Newman installation..." -ForegroundColor $ColorInfo
$newmanInstalled = $null -ne (Get-Command newman -ErrorAction SilentlyContinue)

if (-not $newmanInstalled) {
    Write-Host "Newman is not installed globally." -ForegroundColor $ColorWarning
    Write-Host "Attempting to run with npx (downloads Newman on first run)..." -ForegroundColor $ColorWarning
    $newmanCommand = "npx newman"
} else {
    $newmanVersion = (newman --version 2>&1) | Out-String
    Write-Host "✓ Newman installed: $newmanVersion" -ForegroundColor $ColorSuccess
    $newmanCommand = "newman"
}

# Set up paths
$scriptDir = $PSScriptRoot
$collectionFile = Join-Path $scriptDir "MedManage.postman_collection.json"
$environmentFile = Join-Path $scriptDir "MedManage.postman_environment.json"
$reportsDir = Join-Path $scriptDir "reports"

# Validate files exist
if (-not (Test-Path $collectionFile)) {
    Write-Host "✗ Collection file not found: $collectionFile" -ForegroundColor $ColorError
    exit 1
}

if (-not (Test-Path $environmentFile)) {
    Write-Host "✗ Environment file not found: $environmentFile" -ForegroundColor $ColorError
    exit 1
}

Write-Host "✓ Collection file: $collectionFile" -ForegroundColor $ColorSuccess
Write-Host "✓ Environment: $Environment" -ForegroundColor $ColorSuccess

# Create reports directory
if ($Report -and -not (Test-Path $reportsDir)) {
    New-Item -ItemType Directory -Path $reportsDir -Force | Out-Null
    Write-Host "✓ Created reports directory: $reportsDir" -ForegroundColor $ColorSuccess
}

Write-Host ""
Write-Host "========================================" -ForegroundColor $ColorInfo
Write-Host "Starting Test Execution" -ForegroundColor $ColorInfo
Write-Host "========================================" -ForegroundColor $ColorInfo
Write-Host ""

# Build Newman command
$newmanArgs = @(
    'run',
    "`"$collectionFile`"",
    '-e', "`"$environmentFile`""
)

# Add folder filter if specified
if ($Folder) {
    $newmanArgs += '--folder', "`"$Folder`""
    Write-Host "Running folder: $Folder" -ForegroundColor $ColorInfo
}

# Add SSL skip if needed
if ($SkipSSL) {
    $newmanArgs += '--insecure'
    Write-Host "SSL verification: Disabled (development mode)" -ForegroundColor $ColorWarning
}

# Add verbose flag
if ($Verbose) {
    $newmanArgs += '--verbose'
}

# Add reporters
if ($Report) {
    # Check if htmlextra is installed
    $htmlextraInstalled = $null -ne (Get-Command newman-reporter-htmlextra -ErrorAction SilentlyContinue)
    
    if ($htmlextraInstalled -or $newmanCommand -eq "npx newman") {
        $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
        $reportFile = Join-Path $reportsDir "test-report-$timestamp.html"
        $newmanArgs += '-r', 'cli,htmlextra'
        $newmanArgs += '--reporter-htmlextra-export', "`"$reportFile`""
        Write-Host "HTML Report: $reportFile" -ForegroundColor $ColorInfo
    } else {
        Write-Host "htmlextra reporter not installed. Install with: npm install -g newman-reporter-htmlextra" -ForegroundColor $ColorWarning
        $newmanArgs += '-r', 'cli'
    }
} else {
    $newmanArgs += '-r', 'cli'
}

Write-Host ""

# Execute Newman
try {
    $command = "$newmanCommand $($newmanArgs -join ' ')"
    
    Write-Host "Executing: $newmanCommand" -ForegroundColor $ColorInfo
    Write-Host ""
    
    # Run the command
    Invoke-Expression $command
    
    $exitCode = $LASTEXITCODE
    
    if ($exitCode -eq 0) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor $ColorSuccess
        Write-Host "✓ ALL TESTS PASSED" -ForegroundColor $ColorSuccess
        Write-Host "========================================" -ForegroundColor $ColorSuccess
        
        if ($Report -and (Test-Path $reportFile)) {
            Write-Host ""
            Write-Host "View HTML Report:" -ForegroundColor $ColorInfo
            Write-Host "  $reportFile" -ForegroundColor $ColorWarning
            
            # Offer to open report
            $openReport = Read-Host "Open HTML report in browser? (Y/n)"
            if ($openReport -ne 'n' -and $openReport -ne 'N') {
                Start-Process $reportFile
            }
        }
        
        exit 0
    } else {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor $ColorError
        Write-Host "✗ TESTS FAILED" -ForegroundColor $ColorError
        Write-Host "========================================" -ForegroundColor $ColorError
        
        if ($Report -and (Test-Path $reportFile)) {
            Write-Host ""
            Write-Host "Check HTML report for details:" -ForegroundColor $ColorWarning
            Write-Host "  $reportFile" -ForegroundColor $ColorWarning
        }
        
        exit $exitCode
    }
} catch {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor $ColorError
    Write-Host "✗ ERROR RUNNING TESTS" -ForegroundColor $ColorError
    Write-Host "========================================" -ForegroundColor $ColorError
    Write-Host $_.Exception.Message -ForegroundColor $ColorError
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor $ColorWarning
    Write-Host "  1. Ensure Node.js is installed: node --version" -ForegroundColor $ColorWarning
    Write-Host "  2. Install Newman: npm install -g newman" -ForegroundColor $ColorWarning
    Write-Host "  3. Ensure API is running: https://localhost:58764" -ForegroundColor $ColorWarning
    Write-Host "  4. Check network connectivity" -ForegroundColor $ColorWarning
    
    exit 1
}
