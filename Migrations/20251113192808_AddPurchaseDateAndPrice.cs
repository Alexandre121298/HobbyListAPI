using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HobbyListAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseDateAndPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Books");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Books",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
