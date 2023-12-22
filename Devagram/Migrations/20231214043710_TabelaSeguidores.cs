using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devagram.Migrations
{
    /// <inheritdoc />
    public partial class TabelaSeguidores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seguidores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuarioSeguidor = table.Column<int>(type: "int", nullable: true),
                    idUsuarioSeguido = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguidores", x => x.id);
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_idUsuarioSeguido",
                        column: x => x.idUsuarioSeguido,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_idUsuarioSeguidor",
                        column: x => x.idUsuarioSeguidor,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idUsuarioSeguido",
                table: "Seguidores",
                column: "idUsuarioSeguido");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idUsuarioSeguidor",
                table: "Seguidores",
                column: "idUsuarioSeguidor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seguidores");
        }
    }
}
