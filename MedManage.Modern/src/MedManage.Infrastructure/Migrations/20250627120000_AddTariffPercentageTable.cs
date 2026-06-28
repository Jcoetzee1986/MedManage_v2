using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedManage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTariffPercentageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tariff");

            migrationBuilder.CreateTable(
                name: "TariffPercentage",
                schema: "Tariff",
                columns: table => new
                {
                    TariffPercentageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PercentageAdded = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    TariffPeriodName = table.Column<int>(type: "int", nullable: false),
                    StartActiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndActiveDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecordsAffected = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffPercentage", x => x.TariffPercentageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffPercentage_TariffPeriodName_EndActiveDate",
                schema: "Tariff",
                table: "TariffPercentage",
                columns: new[] { "TariffPeriodName", "EndActiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TariffPercentage_TariffPeriodName_StartActiveDate",
                schema: "Tariff",
                table: "TariffPercentage",
                columns: new[] { "TariffPeriodName", "StartActiveDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffPercentage",
                schema: "Tariff");
        }
    }
}
