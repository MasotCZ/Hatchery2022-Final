using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatcheryFinal_Web_API.Migrations
{
    public partial class CreditSizeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "CreditRequests",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "CreditRequests",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);
        }
    }
}
