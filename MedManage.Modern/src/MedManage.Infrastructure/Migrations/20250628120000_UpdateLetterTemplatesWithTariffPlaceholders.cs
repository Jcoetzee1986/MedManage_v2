using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedManage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLetterTemplatesWithTariffPlaceholders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Replace hardcoded tariff percentage strings in letter templates with Handlebars placeholders.
            // This migration is idempotent: it only performs replacements where the hardcoded text still exists.
            //
            // Known hardcoded patterns in existing templates:
            //   "NHRPL+ 233.90% for 2025" → prior year placeholder
            //   "NHRPL+ 250.60% for 2026" → current year placeholder
            //
            // The full line pattern is:
            //   "Ministry of Health Botswana will pay 2009 NHRPL+ 233.90% for 2025"
            //   "Ministry of Health Botswana will pay 2009 NHRPL+ 250.60% for 2026"
            // And the Botswana/Swaziland variant for referral letters.

            // Replace the prior year hardcoded percentage (233.90% for 2025)
            migrationBuilder.Sql(@"
                UPDATE [shared].[LetterTemplate]
                SET [HtmlContent] = REPLACE(
                    [HtmlContent],
                    N'NHRPL+ 233.90% for 2025',
                    N'NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}'
                )
                WHERE [HtmlContent] LIKE N'%NHRPL+ 233.90[%] for 2025%'
                  AND [HtmlContent] NOT LIKE N'%{{TariffPercentagePriorYearValue}}%';
            ");

            // Replace the current year hardcoded percentage (250.60% for 2026)
            migrationBuilder.Sql(@"
                UPDATE [shared].[LetterTemplate]
                SET [HtmlContent] = REPLACE(
                    [HtmlContent],
                    N'NHRPL+ 250.60% for 2026',
                    N'NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}'
                )
                WHERE [HtmlContent] LIKE N'%NHRPL+ 250.60[%] for 2026%'
                  AND [HtmlContent] NOT LIKE N'%{{TariffPercentageCurrentYearValue}}%';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Handlebars placeholders back to the original hardcoded values.
            // Note: This restores the values as they were at the time of this migration.

            // Revert prior year placeholder back to hardcoded value
            migrationBuilder.Sql(@"
                UPDATE [shared].[LetterTemplate]
                SET [HtmlContent] = REPLACE(
                    [HtmlContent],
                    N'NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}',
                    N'NHRPL+ 233.90% for 2025'
                )
                WHERE [HtmlContent] LIKE N'%{{TariffPercentagePriorYearValue}}%';
            ");

            // Revert current year placeholder back to hardcoded value
            migrationBuilder.Sql(@"
                UPDATE [shared].[LetterTemplate]
                SET [HtmlContent] = REPLACE(
                    [HtmlContent],
                    N'NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}',
                    N'NHRPL+ 250.60% for 2026'
                )
                WHERE [HtmlContent] LIKE N'%{{TariffPercentageCurrentYearValue}}%';
            ");
        }
    }
}
