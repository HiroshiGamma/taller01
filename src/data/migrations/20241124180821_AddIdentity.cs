using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.src.Data.migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Receipts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Receipts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
