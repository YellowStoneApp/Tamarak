using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class CustomerBioAvatarKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarKey",
                table: "Customers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Customers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManuallyEdited",
                table: "Customers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarKey",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ManuallyEdited",
                table: "Customers");
        }
    }
}
