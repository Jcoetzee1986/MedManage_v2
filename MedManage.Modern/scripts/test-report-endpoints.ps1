# ============================================================================
# Report Endpoint Integration Tests
# Run with: .\scripts\test-report-endpoints.ps1 -Username "SystemAdmin" -Password "yourpassword"
# Requires: API running on http://localhost:58765
# NOT part of CI/CD pipeline - manual testing only.
# ============================================================================

param(
    [Parameter(Mandatory=$true)]
    [string]$Username,
    [Parameter(Mandatory=$true)]
    [string]$Password,
    [string]$BaseUrl = "http://localhost:58765"
)

$ErrorActionPreference = "Continue"
$passed = 0
$failed = 0
$results = @()

function Test-Endpoint {
    param(
        [string]$Name,
        [string]$Url,
        [string]$Body,
        [string]$Token,
        [int[]]$ExpectedStatuses = @(200)
    )

    try {
        $response = Invoke-WebRequest -Uri $Url -Method POST -Headers @{
            "Authorization" = "Bearer $Token"
            "Content-Type" = "application/json"
        } -Body $Body -UseBasicParsing

        $status = $response.StatusCode
        $size = $response.Content.Length

        if ($status -in $ExpectedStatuses) {
            Write-Host "  PASS  $Name - Status: $status | Size: $size bytes" -ForegroundColor Green
            $script:passed++
            return @{ Name=$Name; Status="PASS"; Code=$status; Size=$size }
        } else {
            Write-Host "  FAIL  $Name - Unexpected status: $status" -ForegroundColor Red
            $script:failed++
            return @{ Name=$Name; Status="FAIL"; Code=$status; Size=$size }
        }
    } catch {
        $errStatus = 0
        $errBody = ""
        if ($_.Exception.Response) {
            $errStatus = [int]$_.Exception.Response.StatusCode
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = [System.IO.StreamReader]::new($stream)
            $errBody = $reader.ReadToEnd()
        }

        if ($errStatus -in $ExpectedStatuses) {
            Write-Host "  PASS  $Name - Status: $errStatus (expected)" -ForegroundColor Green
            $script:passed++
            return @{ Name=$Name; Status="PASS"; Code=$errStatus }
        } else {
            $shortBody = if ($errBody.Length -gt 200) { $errBody.Substring(0,200) + "..." } else { $errBody }
            Write-Host "  FAIL  $Name - Status: $errStatus | $shortBody" -ForegroundColor Red
            $script:failed++
            return @{ Name=$Name; Status="FAIL"; Code=$errStatus; Error=$shortBody }
        }
    }
}

# ─── Login ─────────────────────────────────────────────────────
Write-Host ""
Write-Host "Authenticating..." -ForegroundColor Cyan
try {
    $loginResp = Invoke-WebRequest -Uri "$BaseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body "{`"username`":`"$Username`",`"password`":`"$Password`"}" -UseBasicParsing
    $loginJson = $loginResp.Content | ConvertFrom-Json
    if (-not $loginJson.success) {
        Write-Host "Login failed: $($loginJson.message)" -ForegroundColor Red
        exit 1
    }
    $token = $loginJson.token
    Write-Host "Authenticated successfully." -ForegroundColor Green
} catch {
    Write-Host "Login request failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " Report Endpoint Tests (api/report)" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ─── Test 1: Cases Between Dates - PDF ──────────────────────────
$results += Test-Endpoint -Name "Cases Between Dates (PDF)" `
    -Url "$BaseUrl/api/report/cases-between-dates" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"pdf"}' `
    -Token $token

# ─── Test 2: Cases Between Dates - Excel ────────────────────────
$results += Test-Endpoint -Name "Cases Between Dates (Excel)" `
    -Url "$BaseUrl/api/report/cases-between-dates" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"excel"}' `
    -Token $token

# ─── Test 3: Cases Between Dates - with filters ────────────────
$results += Test-Endpoint -Name "Cases Between Dates (with filters)" `
    -Url "$BaseUrl/api/report/cases-between-dates" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","statusId":1,"caseTypeId":1,"format":"pdf"}' `
    -Token $token

# ─── Test 4: Billing Summary - Excel ───────────────────────────
$results += Test-Endpoint -Name "Billing Summary (Excel)" `
    -Url "$BaseUrl/api/report/billing-summary" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"excel"}' `
    -Token $token

# ─── Test 5: Billing Summary - PDF ─────────────────────────────
$results += Test-Endpoint -Name "Billing Summary (PDF)" `
    -Url "$BaseUrl/api/report/billing-summary" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"pdf"}' `
    -Token $token

# ─── Test 6: WIP Extract - Excel ───────────────────────────────
$results += Test-Endpoint -Name "WIP Extract (Excel)" `
    -Url "$BaseUrl/api/report/wip-extract" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"excel"}' `
    -Token $token

# ─── Test 7: WIP Extract - PDF ─────────────────────────────────
$results += Test-Endpoint -Name "WIP Extract (PDF)" `
    -Url "$BaseUrl/api/report/wip-extract" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28","format":"pdf"}' `
    -Token $token

# ─── Test 8: Tariff Detail - PDF ───────────────────────────────
$results += Test-Endpoint -Name "Tariff Detail (PDF)" `
    -Url "$BaseUrl/api/report/tariff-detail" `
    -Body '{"caseId":190879,"format":"pdf"}' `
    -Token $token

# ─── Test 9: Tariff Detail - Excel ─────────────────────────────
$results += Test-Endpoint -Name "Tariff Detail (Excel)" `
    -Url "$BaseUrl/api/report/tariff-detail" `
    -Body '{"caseId":190879,"format":"excel"}' `
    -Token $token

# ─── Test 10: My Cases - Excel ─────────────────────────────────
$results += Test-Endpoint -Name "My Cases (Excel)" `
    -Url "$BaseUrl/api/report/my-cases" `
    -Body '{"format":"excel"}' `
    -Token $token

# ─── Test 11: Linked Cases - PDF ───────────────────────────────
$results += Test-Endpoint -Name "Linked Cases (PDF)" `
    -Url "$BaseUrl/api/report/linked-cases" `
    -Body '{"caseId":190879,"format":"pdf"}' `
    -Token $token

# ─── Test 12: Case Comments - Excel ────────────────────────────
$results += Test-Endpoint -Name "Case Comments (Excel)" `
    -Url "$BaseUrl/api/report/case-comments" `
    -Body '{"caseId":190879,"format":"excel"}' `
    -Token $token

# ─── Test 13: Missing required dates (should 400) ──────────────
$results += Test-Endpoint -Name "Cases Between Dates (missing dates = 400)" `
    -Url "$BaseUrl/api/report/cases-between-dates" `
    -Body '{"format":"pdf"}' `
    -Token $token `
    -ExpectedStatuses @(200, 400)

# ─── Test 14: Default format (no format field) ─────────────────
$results += Test-Endpoint -Name "Cases Between Dates (default format)" `
    -Url "$BaseUrl/api/report/cases-between-dates" `
    -Body '{"dateFrom":"2026-01-01","dateTo":"2026-06-28"}' `
    -Token $token

# ─── Summary ──────────────────────────────────────────────────
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " Results: $passed passed, $failed failed (total: $($passed + $failed))" -ForegroundColor $(if ($failed -eq 0) { "Green" } else { "Yellow" })
Write-Host "============================================" -ForegroundColor Cyan

if ($failed -gt 0) { exit 1 }
