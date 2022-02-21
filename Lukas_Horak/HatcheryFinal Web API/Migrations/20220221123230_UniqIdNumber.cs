using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatcheryFinal_Web_API.Migrations
{
    public partial class UniqIdNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CreditPartners_IdNumber",
                table: "CreditPartners",
                column: "IdNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CreditPartners_IdNumber",
                table: "CreditPartners");
        }
    }
}
