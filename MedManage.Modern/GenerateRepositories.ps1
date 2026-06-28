# Repository Generator Script
# This script generates basic repository implementations for all entities

$repositories = @(
    @{Name="Country"; Methods=@("GetActiveAsync", "GetByCountryNameAsync")},
    @{Name="Gender"; Methods=@("GetActiveAsync")},
    @{Name="Language"; Methods=@("GetActiveAsync")},
    @{Name="MarritalStatus"; Methods=@("GetActiveAsync")},
    @{Name="Title"; Methods=@("GetActiveAsync")},
    @{Name="Race"; Methods=@("GetActiveAsync")},
    @{Name="BillingStatus"; Methods=@("GetActiveAsync")},
    @{Name="CaseStatus"; Methods=@("GetActiveAsync", "GetByDescriptionAsync")},
    @{Name="CaseType"; Methods=@("GetActiveAsync", "GetForFiltersAsync")},
    @{Name="FacilityType"; Methods=@("GetActiveAsync")},
    @{Name="MemberStatus"; Methods=@("GetActiveAsync")},
    @{Name="Speciality"; Methods=@("GetActiveAsync")},
    @{Name="Cpt"; Methods=@("SearchByFiltersAsync", "GetByCptCodeAsync", "GetActiveCodesAsync")},
    @{Name="Icd"; Methods=@("SearchByFiltersAsync", "GetByIcdCodeAsync", "GetActiveCodesAsync")},
    @{Name="NappiCode"; Methods=@("SearchAsync", "GetByNappiCodeAsync", "GetActiveCodesAsync")},
    @{Name="MedicalAid"; Methods=@("GetByIdWithDetailsAsync", "GetByMedicalAidIdAsync", "GetActiveAsync")},
    @{Name="MedicalAidProduct"; Methods=@("GetByMedicalAidIdAsync", "GetActiveAsync")},
    @{Name="Exclusion"; Methods=@("GetActiveAsync")},
    @{Name="Booking"; Methods=@("GetByCaseIdAsync", "GetByMemberNumberAsync", "GetByBookingIdAsync", "SearchByFiltersAsync")},
    @{Name="Episode"; Methods=@("GetByIdWithCasesAsync", "SearchByFiltersAsync")},
    @{Name="CaseBilling"; Methods=@("GetByCaseIdAsync", "GetNotLinkedToCaseAsync", "GetByServiceProviderIdAsync")},
    @{Name="CaseChecklist"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseComment"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseCpt"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseDiscount"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseExclusion"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseFacilityType"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseIcd"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseLetterNote"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseLink"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseLinkedFile"; Methods=@("GetByCaseIdAsync", "DeleteByCaseLinkedFileIdAsync")},
    @{Name="CaseNappiCode"; Methods=@("GetByCaseIdAsync")},
    @{Name="CaseNote"; Methods=@("GetByCaseIdAsync", "GetLastNoteByCaseIdAsync")},
    @{Name="CaseTariff"; Methods=@("GetByCaseIdAsync", "GetForReportAsync")},
    @{Name="MemberChronicIllness"; Methods=@("GetByMemberIdAsync")},
    @{Name="MemberMedicalAidProduct"; Methods=@("GetByMemberIdAsync")},
    @{Name="MemberNote"; Methods=@("GetByMemberIdAsync")},
    @{Name="ChecklistTemplate"; Methods=@("GetActiveAsync")},
    @{Name="LinkedFile"; Methods=@("GetByEntityIdAndTypeAsync", "DeleteByLinkedFileIdAsync")},
    @{Name="BaseTariff"; Methods=@("GetAllAsync", "GetNewCustomCodeAsync", "InsertCustomAsync")},
    @{Name="ServiceProviderTariff"; Methods=@("GetByIdWithDetailsAsync", "GetByServiceProviderIdAsync")},
    @{Name="ServiceProviderTariffCustom"; Methods=@("GetByServiceProviderIdAsync", "InsertFromExcelAsync")},
    @{Name="MedicalAidExclusion"; Methods=@("GetByMedicalAidIdAsync", "InsertBySpecialityAsync", "InsertByHospitalTypeAsync")},
    @{Name="EpisodeCase"; Methods=@("GetByEpisodeIdAsync")},
    @{Name="SessionUserCase"; Methods=@("GetByCaseIdAsync")}
)

$outputPath = "c:\Code\MedManage\MedManage\MedManage.Modern\src\MedManage.Infrastructure\Repositories"

foreach ($repo in $repositories) {
    $name = $repo.Name
    $fileName = "$outputPath\$($name)Repository.cs"
    
    # Skip if already exists
    if (Test-Path $fileName) {
        Write-Host "Skipping $name - already exists"
        continue
    }
    
    $content = @"
using Microsoft.EntityFrameworkCore;
using MedManage.Core.Interfaces.Repositories;
using MedManage.Infrastructure.Data;
using MedManage.Infrastructure.Entities;

namespace MedManage.Infrastructure.Repositories;

public class $($name)Repository : Repository<$name>, I$($name)Repository
{
    public $($name)Repository(MedManageDbContext context) : base(context)
    {
    }

    // TODO: Implement specific methods for this repository
    // Methods to implement: $($repo.Methods -join ", ")
}
"@
    
    $content | Out-File -FilePath $fileName -Encoding UTF8
    Write-Host "Created $fileName"
}

Write-Host "`nRepository generation complete!"
