<#
.SYNOPSIS
    Builds and starts all MedManage.Modern containers using Podman.

.DESCRIPTION
    Runs podman-compose to build the API and Angular frontend containers.
    The SQL Server database is expected to be running on the host machine.

.PARAMETER Build
    Force rebuild of container images.

.PARAMETER Detach
    Run containers in the background (detached mode).

.EXAMPLE
    .\scripts\podman-up.ps1
    .\scripts\podman-up.ps1 -Build -Detach
#>

param(
    [switch]$Build,
    [switch]$Detach
)

$ErrorActionPreference = 'Stop'
$ProjectRoot = Split-Path -Parent $PSScriptRoot

Push-Location $ProjectRoot
try {
    # Check prerequisites
    if (-not (Get-Command podman -ErrorAction SilentlyContinue)) {
        Write-Error "Podman is not installed or not in PATH. Install from https://podman.io/getting-started/installation"
        exit 1
    }

    if (-not (Get-Command podman-compose -ErrorAction SilentlyContinue)) {
        Write-Host "Installing podman-compose via pip..." -ForegroundColor Yellow
        pip install podman-compose
    }

    # Check .env file exists
    if (-not (Test-Path ".env")) {
        Write-Host "No .env file found. Creating from .env.example..." -ForegroundColor Yellow
        Copy-Item ".env.example" ".env"
        Write-Host "Please edit .env with your database password and JWT secret, then re-run this script." -ForegroundColor Red
        exit 1
    }

    # Verify SQL Server is accessible on host
    Write-Host "Verifying SQL Server connectivity..." -ForegroundColor Cyan
    $connTest = Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue
    if (-not $connTest.TcpTestSucceeded) {
        Write-Warning "SQL Server does not appear to be listening on localhost:1433. Ensure it is running and accepting TCP connections."
    } else {
        Write-Host "  SQL Server is reachable on port 1433" -ForegroundColor Green
    }

    # Build args
    $composeArgs = @("up")
    if ($Build) { $composeArgs += "--build" }
    if ($Detach) { $composeArgs += "-d" }

    Write-Host "`nStarting MedManage.Modern containers..." -ForegroundColor Cyan
    Write-Host "  API:       http://localhost:5000" -ForegroundColor White
    Write-Host "  Frontend:  http://localhost:8080" -ForegroundColor White
    Write-Host ""

    podman-compose @composeArgs
}
finally {
    Pop-Location
}
