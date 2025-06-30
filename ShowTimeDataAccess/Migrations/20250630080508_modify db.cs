using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowTime.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class modifydb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Festival",
                columns: new[] { "Id", "Capacity", "EndDate", "Location", "Name", "SplashArt", "StartDate" },
                values: new object[] { 1, 100000, new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "England", "Glastonbury", "images/Glastologo.png", new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Artists");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
