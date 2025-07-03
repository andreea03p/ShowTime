using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShowTime.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Festival",
                newName: "Address");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Festival",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Festival",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Festival");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Festival",
                newName: "Location");

            migrationBuilder.InsertData(
                table: "Festival",
                columns: new[] { "Id", "Capacity", "EndDate", "Location", "Name", "SplashArt", "StartDate" },
                values: new object[,]
                {
                    { 1, 200000, new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "England", "Glastonbury", "images/Glastologo.png", new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 200000, new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bontida", "ElectricCastle", "images/ec.jpg", new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 400000, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClujNapoca", "Untold", "images/untold.png", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 50000, new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Austria", "NovaRock", "images/novalogo.png", new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 800000, new DateTime(2026, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "England", "Coachella", "images/coachella.png", new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 200000, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Budapest", "Sziget", "images/sziget.jpg", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 500000, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Belgium", "Tomorrowland", "images/tmrwl.png", new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 500000, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bucharest", "SummerWell", "images/summerwell.png", new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
