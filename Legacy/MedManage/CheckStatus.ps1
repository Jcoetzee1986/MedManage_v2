$entitiesPath = "c:\Code\MedManage\MedManage\MedManage.Modern\src\MedManage.Infrastructure\Entities"
$allFiles = Get-ChildItem -Path $entitiesPath -Filter "*.cs" | Where-Object { $_.Name -notlike "Vw*" }
$needsRefactoring = @()
$alreadyDone = @()

foreach ($file in $allFiles) {
    $content = Get-Content $file.FullName -Raw
    $hasBase = $content -match ': BaseEntity'
    $hasAudit = $content -match 'public.*DateTime.*DateInserted'
    
    if ($hasAudit -and -not $hasBase) {
        $needsRefactoring += $file.Name
    } elseif ($hasBase) {
        $alreadyDone += $file.Name
    }
}

Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Already has BaseEntity: $($alreadyDone.Count)" -ForegroundColor Green
Write-Host "  Needs refactoring: $($needsRefactoring.Count)" -ForegroundColor Yellow
Write-Host ""

if ($needsRefactoring.Count -gt 0) {
    Write-Host "Files needing refactoring:" -ForegroundColor Yellow
    $needsRefactoring | ForEach-Object { Write-Host "  - $_" }
}
