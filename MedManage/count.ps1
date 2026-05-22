$path = "c:\Code\MedManage\MedManage\MedManage.Modern\src\MedManage.Infrastructure\Entities"
$count = 0
Get-ChildItem "$path\*.cs" | Where-Object { $_.Name -notlike "Vw*" } | ForEach-Object {
    $txt = Get-Content $_.FullName -Raw
    if (($txt -match 'DateTime') -and ($txt -notmatch ': BaseEntity')) {
        $count++
    }
}
Write-Output "Files to refactor: $count"
