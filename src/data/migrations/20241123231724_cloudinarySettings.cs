using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.src.Data.migrations
{
    /// <inheritdoc />
    public partial class cloudinarySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Estados_EstadoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EstadoId",
                table: "Users",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "Correo",
                table: "Users",
                newName: "Mail");

            migrationBuilder.RenameColumn(
                name: "Contrasena",
                table: "Users",
                newName: "Birthdate");

            migrationBuilder.RenameIndex(
                name: "IX_Users_EstadoId",
                table: "Users",
                newName: "IX_Users_StatusId");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Products",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Products",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Statuses_StatusId",
                table: "Users",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Statuses_StatusId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Users",
                newName: "EstadoId");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "FechaNacimiento");

            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "Users",
                newName: "Correo");

            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Users",
                newName: "Contrasena");

            migrationBuilder.RenameIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                newName: "IX_Users_EstadoId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Products",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Nombre");

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Estados_EstadoId",
                table: "Users",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
