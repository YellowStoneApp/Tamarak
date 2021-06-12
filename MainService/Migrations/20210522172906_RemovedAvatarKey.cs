using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class RemovedAvatarKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarKey",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarKey",
                table: "Customers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
