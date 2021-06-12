using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainService.Migrations
{
    public partial class MovedDateAddedToCustomerGift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Gifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "CustomerGifts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "CustomerGifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Gifts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
