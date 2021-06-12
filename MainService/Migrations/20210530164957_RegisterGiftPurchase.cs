using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class RegisterGiftPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGiftPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerGiftId = table.Column<int>(type: "int", nullable: true),
                    PurchaserEmail = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConfirmedPurchase = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConfirmationRequestTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    GiftPurchaseRequestTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGiftPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGiftPurchases_CustomerGifts_CustomerGiftId",
                        column: x => x.CustomerGiftId,
                        principalTable: "CustomerGifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGiftPurchases_CustomerGiftId",
                table: "CustomerGiftPurchases",
                column: "CustomerGiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGiftPurchases");
        }
    }
}
