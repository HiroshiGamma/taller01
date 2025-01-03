using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.src.data.migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "04ea9f62-8d4e-423a-b771-3fd56e5915b3", "AQAAAAIAAYagAAAAECPvUXLe4tP9d4NNYVoByDdFkSH+K4CGuQxgZMKBBMVJ1EN8jhZsUN9GLma0pyxR2A==", "9c0b4b75-b2e2-43df-87d8-30ccabb0448d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8bce8f5-7eb7-4e95-876e-3fb8d1461e0c", "AQAAAAIAAYagAAAAEJW2kJqsGSGvaCdrDkb1RsmXCphxDsCDgMALhYlvpAh8M+JL99+LAu7o2Wpkz5OeoQ==", "b59edbb6-8656-4f64-aec0-766677f863e8" });
        }
    }
}
