$entitiesPath = "c:\Code\MedManage\MedManage\MedManage.Modern\src\MedManage.Infrastructure\Entities"
$files = Get-ChildItem -Path $entitiesPath -Filter "*.cs" | Where-Object { 
    $_.Name -notlike "Vw*"  # Exclude views
}

$refactoredCount = 0
$skippedCount = 0
$errors = @()

foreach ($file in $files) {
    try {
        $content = Get-Content -Path $file.FullName -Raw
        $originalContent = $content
        
        # Check if this file has DateInserted property
        if ($content -match 'public DateTime(\?)? DateInserted') {
            Write-Host "Processing: $($file.Name)" -ForegroundColor Cyan
            
            # Add using statement if not present
            if ($content -notmatch 'using MedManage\.Core\.Entities;') {
                $content = $content -replace '(namespace MedManage\.Infrastructure\.Entities;)', "using MedManage.Core.Entities;`n`n`$1"
            }
            
            # Add BaseEntity inheritance to class declaration (handle different patterns)
            $content = $content -replace '(public partial class \w+)\s*(\r?\n\{)', '$1 : BaseEntity$2'
            
            # Remove UserID property with all its attributes (before DateUpdated pattern)
            $content = $content -replace '(?s)\s+\[Column\("UserID"\)\]\s+\[StringLength\(\d+\)\]\s+(\[Unicode\(false\)\]\s+)?public string\? UserId \{ get; set; \}\s+', "`n`n    "
            
            # Remove DateUpdated property with its attribute
            $content = $content -replace '(?s)\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime\? DateUpdated \{ get; set; \}\s+', "`n`n    "
            
            # Remove UpdatedUserID property with all its attributes
            $content = $content -replace '(?s)\s+\[Column\("UpdatedUserID"\)\]\s+\[StringLength\(\d+\)\]\s+public string\? UpdatedUserId \{ get; set; \}\s+', "`n`n    "
            
            # Remove DateInserted property with its attribute (at the end pattern)
            $content = $content -replace '(?s)\s+\[Column\(TypeName = "datetime"\)\]\s+public DateTime(\?)? DateInserted \{ get; set; \}\s+', "`n`n    "
            
            # Also handle DateInserted without Column attribute
            $content = $content -replace '(?s)\s+public DateTime(\?)? DateInserted \{ get; set; \}\s+', "`n`n    "
            
            # Also handle UserID without Unicode attribute
            $content = $content -replace '(?s)\s+\[Column\("UserID"\)\]\s+\[StringLength\(\d+\)\]\s+public string\? UserId \{ get; set; \}\s+', "`n`n    "
            
            # Clean up excessive whitespace before closing brace or next property
            $content = $content -replace '(\r?\n\s*){3,}(\r?\n\s+\[)', "`n`n`$2"
            $content = $content -replace '(\r?\n\s*){3,}(\r?\n\s+public)', "`n`n`$2"
            $content = $content -replace '(\r?\n\s*){3,}(\r?\n\})', "`n`$2"
            
            if ($content -ne $originalContent) {
                Set-Content -Path $file.FullName -Value $content -NoNewline
                $refactoredCount++
                Write-Host "  ✓ Refactored" -ForegroundColor Green
            } else {
                Write-Host "  - No changes made" -ForegroundColor Yellow
                $skippedCount++
            }
        } else {
            $skippedCount++
        }
    } catch {
        $errors += "Error processing $($file.Name): $_"
        Write-Host "  ✗ Error: $_" -ForegroundColor Red
    }
}

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "Refactoring Complete!" -ForegroundColor Green
Write-Host "  Refactored: $refactoredCount files" -ForegroundColor Green
Write-Host "  Skipped: $skippedCount files" -ForegroundColor Gray
if ($errors.Count -gt 0) {
    Write-Host "  Errors: $($errors.Count)" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host "    - $_" -ForegroundColor Red }
}
Write-Host "========================================" -ForegroundColor Green
