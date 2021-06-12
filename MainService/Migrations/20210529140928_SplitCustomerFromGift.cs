using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class SplitCustomerFromGift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Customers_CustomerId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_CustomerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Gifts");

            migrationBuilder.CreateTable(
                name: "CustomerGifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    GiftId = table.Column<int>(type: "int", nullable: false),
                    CustomDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGifts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerGifts_Gifts_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGifts_CustomerId",
                table: "CustomerGifts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGifts_GiftId",
                table: "CustomerGifts",
                column: "GiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGifts");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_CustomerId",
                table: "Gifts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Customers_CustomerId",
                table: "Gifts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
