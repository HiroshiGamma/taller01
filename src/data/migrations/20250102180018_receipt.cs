using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.src.data.migrations
{
    /// <inheritdoc />
    public partial class receipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Receipts_ReceiptId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ReceiptId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Receipts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Commune",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "Receipts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRut",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReceiptItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Total = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptItem_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptItem_Receipts_Id",
                        column: x => x.Id,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Enabled", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Rut", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "d31306a8-8b04-4042-8a9c-cc7cd8c1022a", new DateTime(2000, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@idwm.cl", true, true, "Masculino", false, null, "ADMIN@IDWM.CL", "IGNACIOMANCILLA", "AQAAAAIAAYagAAAAEDNh+xZOhVgerQetpodpUub3AXuMEF6lIh3DsZZqoHOYWW1weml+pgwXMbbzRHBgyw==", null, false, "20416699-4", "3cc3cc3d-5afe-4063-a2aa-888a9afba6ef", false, "IgnacioMancilla" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_UserId",
                table: "Receipts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItem_ProductId",
                table: "ReceiptItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItem_ProductId1",
                table: "ReceiptItem",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_AspNetUsers_UserId",
                table: "Receipts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_AspNetUsers_UserId",
                table: "Receipts");

            migrationBuilder.DropTable(
                name: "ReceiptItem");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_UserId",
                table: "Receipts");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Commune",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "UserRut",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiptId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReceiptId",
                table: "Products",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Receipts_ReceiptId",
                table: "Products",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id");
        }
    }
}
