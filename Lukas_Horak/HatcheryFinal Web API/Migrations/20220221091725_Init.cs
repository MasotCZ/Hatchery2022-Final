using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatcheryFinal_Web_API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditRequests",
                columns: table => new
                {
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    MonthsTillCreditMaturity = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequests", x => x.Token);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditRequests");
        }
    }
}
