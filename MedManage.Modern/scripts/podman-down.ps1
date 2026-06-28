<#
.SYNOPSIS
    Stops and removes all MedManage.Modern containers.

.PARAMETER Volumes
    Also remove named volumes (uploads, logs).

.EXAMPLE
    .\scripts\podman-down.ps1
    .\scripts\podman-down.ps1 -Volumes
#>

param(
    [switch]$Volumes
)

$ErrorActionPreference = 'Stop'
$ProjectRoot = Split-Path -Parent $PSScriptRoot

Push-Location $ProjectRoot
try {
    $args = @("down")
    if ($Volumes) { $args += "-v" }

    Write-Host "Stopping MedManage.Modern containers..." -ForegroundColor Cyan
    podman-compose @args
    Write-Host "Done." -ForegroundColor Green
}
finally {
    Pop-Location
}
