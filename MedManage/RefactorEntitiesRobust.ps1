param(
    [switch]$WhatIf = $false
)

$entitiesPath = "c:\Code\MedManage\MedManage\MedManage.Modern\src\MedManage.Infrastructure\Entities"
$files = Get-ChildItem -Path $entitiesPath -Filter "*.cs" | Where-Object { 
    $_.Name -notlike "Vw*" # Exclude views
}

$refactoredFiles = @()
$skippedFiles = @()
$alreadyDone = @()
$errors = @()

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Entity Refactoring Script" -ForegroundColor Cyan
if ($WhatIf) {
    Write-Host "Mode: DRY RUN (no changes)" -ForegroundColor Yellow
} else {
    Write-Host "Mode: LIVE (will modify files)" -ForegroundColor Green
}
Write-Host "============================================`n" -ForegroundColor Cyan

foreach ($file in $files) {
    try {
        $content = Get-Content -Path $file.FullName -Raw
        $originalContent = $content
        $modified = $false
        
        # Skip if already inherits from BaseEntity
        if ($content -match 'public partial class \w+ : BaseEntity') {
            $alreadyDone += $file.Name
            continue
        }
        
        # Check if file has audit properties
        $hasAudit = $content -match 'public DateTime(\?)? DateInserted'
        
        if (-not $hasAudit) {
            $skippedFiles += $file.Name
            continue
        }
        
        Write-Host "Processing: $($file.Name)" -ForegroundColor Cyan
        
        # Step 1: Add using statement if not present
        if ($content -notmatch 'using MedManage\.Core\.Entities;') {
            $content = $content -replace '(using Microsoft\.EntityFrameworkCore;)', "`$1`nusing MedManage.Core.Entities;"
            $modified = $true
            Write-Host "  + Added using statement" -ForegroundColor DarkGray
        }
        
        # Step 2: Add BaseEntity inheritance
        if ($content -match '(public partial class \w+)\s*(\r?\n\{)') {
            $content = $content -replace '(public partial class \w+)\s*(\r?\n\{)', '$1 : BaseEntity$2'
            $modified = $true
            Write-Host "  + Added BaseEntity inheritance" -ForegroundColor DarkGray
        }
        
        # Step 3: Remove audit properties - handle various patterns
        
        # Pattern 1: UserID with attributes (before other properties)
        if ($content -match '\s+\[Column\("UserID"\)\]\s+\[StringLength\(\d+\)\](\s+\[Unicode\(false\)\])?\s+public string\? UserId \{ get; set; \}') {
            $content = $content -replace '\s+\[Column\("UserID"\)\]\s+\[StringLength\(\d+\)\](\s+\[Unicode\(false\)\])?\s+public string\? UserId \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed UserID property" -ForegroundColor DarkGray
        }
        
        # Pattern 2: DateUpdated with attribute
        if ($content -match '\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime\? DateUpdated \{ get; set; \}') {
            $content = $content -replace '\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime\? DateUpdated \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed DateUpdated property" -ForegroundColor DarkGray
        }
        
        # Pattern 3: UpdatedUserID with attributes
        if ($content -match '\s+\[Column\("UpdatedUserID"\)\]\s+\[StringLength\(\d+\)\]\s+public string\? UpdatedUserId \{ get; set; \}') {
            $content = $content -replace '\s+\[Column\("UpdatedUserID"\)\]\s+\[StringLength\(\d+\)\]\s+public string\? UpdatedUserId \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed UpdatedUserID property" -ForegroundColor DarkGray
        }
        
        # Pattern 4: DateInserted with attribute (DateTime not nullable)
        if ($content -match '\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime DateInserted \{ get; set; \}') {
            $content = $content -replace '\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime DateInserted \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed DateInserted property (non-nullable)" -ForegroundColor DarkGray
        }
        
        # Pattern 5: DateInserted without Column attribute (DateTime not nullable)
        if ($content -match '\s+public DateTime DateInserted \{ get; set; \}') {
            $content = $content -replace '\s+public DateTime DateInserted \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed DateInserted property (no attribute)" -ForegroundColor DarkGray
        }
        
        # Pattern 6: DateInserted nullable (DateTime?)  
        if ($content -match '\s+public DateTime\? DateInserted \{ get; set; \}') {
            $content = $content -replace '\s+public DateTime\? DateInserted \{ get; set; \}\s+', "`n`n    "
            $modified = $true
            Write-Host "  - Removed DateInserted property (nullable)" -ForegroundColor DarkGray
        }
        
        # Clean up excessive blank lines
        $content = $content -replace '(\r?\n){4,}', "`n`n"
        $content = $content -replace '(\r?\n\s*){3,}(\r?\n\s+public)', "`n`n`$2"
        $content = $content -replace '(\r?\n\s*){3,}(\r?\n\s+\[)', "`n`n`$2"
        $content = $content -replace '(\r?\n\s*){3,}(\r?\n\})', "`n`$2"
        
        # Save if modified
        if ($modified -and ($content -ne $originalContent)) {
            if (-not $WhatIf) {
                Set-Content -Path $file.FullName -Value $content -NoNewline
            }
            $refactoredFiles += $file.Name
            Write-Host "  ✓ Refactored" -ForegroundColor Green
        } else {
            if ($modified) {
                Write-Host "  - Changes did not alter content" -ForegroundColor Yellow
            }
            $skippedFiles += $file.Name
        }
        
    } catch {
        $errors += "Error processing $($file.Name): $_"
        Write-Host "  ✗ Error: $_" -ForegroundColor Red
    }
}

Write-Host "`n============================================" -ForegroundColor Cyan
Write-Host "Refactoring Summary" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Refactored:     $($refactoredFiles.Count) files" -ForegroundColor Green
Write-Host "  Already done:   $($alreadyDone.Count) files" -ForegroundColor Gray
Write-Host "  Skipped:        $($skippedFiles.Count) files" -ForegroundColor Gray
if ($errors.Count -gt 0) {
    Write-Host "  Errors:         $($errors.Count)" -ForegroundColor Red
} else {
    Write-Host "  Errors:         $($errors.Count)" -ForegroundColor Gray
}
Write-Host "============================================`n" -ForegroundColor Cyan

if ($WhatIf) {
    Write-Host "NOTE: This was a DRY RUN. No files were modified." -ForegroundColor Yellow
    Write-Host "To apply changes, run without -WhatIf parameter.`n" -ForegroundColor Yellow
}

if ($errors.Count -gt 0) {
    Write-Host "Errors encountered:" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
}

# Show some already done examples
if ($alreadyDone.Count -gt 0) {
    Write-Host "`nAlready refactored (sample):" -ForegroundColor Gray
    $alreadyDone | Select-Object -First 5 | ForEach-Object { Write-Host "  - $_" -ForegroundColor DarkGray }
    if ($alreadyDone.Count -gt 5) {
        $remaining = $alreadyDone.Count - 5
        Write-Host "  ... and $remaining more" -ForegroundColor DarkGray
    }
}
