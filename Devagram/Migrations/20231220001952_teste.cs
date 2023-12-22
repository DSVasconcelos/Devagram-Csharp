using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devagram.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seguidores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuarioSeguido = table.Column<int>(type: "int", nullable: true),
                    idUsuarioSeguidor = table.Column<int>(type: "int", nullable: true)
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
                name: "IX_Publicacoes_IdUsuario",
                table: "Publicacoes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idUsuarioSeguido",
                table: "Seguidores",
                column: "idUsuarioSeguido");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idUsuarioSeguidor",
                table: "Seguidores",
                column: "idUsuarioSeguidor");
        }
    }
}
