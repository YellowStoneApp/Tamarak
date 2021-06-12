using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class AddGiftState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftState",
                table: "CustomerGifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiftState",
                table: "CustomerGifts");
        }
    }
}
