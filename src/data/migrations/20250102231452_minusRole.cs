using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.src.data.migrations
{
    /// <inheritdoc />
    public partial class minusRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.AlterColumn<string>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8bce8f5-7eb7-4e95-876e-3fb8d1461e0c", "25-10-2000", "AQAAAAIAAYagAAAAEJW2kJqsGSGvaCdrDkb1RsmXCphxDsCDgMALhYlvpAh8M+JL99+LAu7o2Wpkz5OeoQ==", "b59edbb6-8656-4f64-aec0-766677f863e8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3", null, "Disabled", "DISABLED" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d31306a8-8b04-4042-8a9c-cc7cd8c1022a", new DateTime(2000, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEDNh+xZOhVgerQetpodpUub3AXuMEF6lIh3DsZZqoHOYWW1weml+pgwXMbbzRHBgyw==", "3cc3cc3d-5afe-4063-a2aa-888a9afba6ef" });
        }
    }
}
