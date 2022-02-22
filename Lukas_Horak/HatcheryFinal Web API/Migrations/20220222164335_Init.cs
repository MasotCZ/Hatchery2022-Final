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
                name: "CreditPartners",
                columns: table => new
                {
                    IdNumber = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPartners", x => x.IdNumber);
                    table.UniqueConstraint("AK_CreditPartners_Token", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "CreditRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    ContactNotes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequestStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    CreditLengthInMonths = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditRequests_CreditPartners_Token",
                        column: x => x.Token,
                        principalTable: "CreditPartners",
                        principalColumn: "Token",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditRequests_CreditRequestStatus_ContactStatusId",
                        column: x => x.ContactStatusId,
                        principalTable: "CreditRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequests_ContactStatusId",
                table: "CreditRequests",
                column: "ContactStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequests_Token",
                table: "CreditRequests",
                column: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditRequests");

            migrationBuilder.DropTable(
                name: "CreditPartners");

            migrationBuilder.DropTable(
                name: "CreditRequestStatus");
        }
    }
}
