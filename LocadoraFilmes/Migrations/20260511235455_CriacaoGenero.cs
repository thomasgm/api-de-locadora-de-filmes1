using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraFilmes.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoGenero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Filmes");

            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmeGenero",
                columns: table => new
                {
                    FilmesId = table.Column<int>(type: "int", nullable: false),
                    GenerosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmeGenero", x => new { x.FilmesId, x.GenerosId });
                    table.ForeignKey(
                        name: "FK_FilmeGenero_Filmes_FilmesId",
                        column: x => x.FilmesId,
                        principalTable: "Filmes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmeGenero_Genero_GenerosId",
                        column: x => x.GenerosId,
                        principalTable: "Genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmeGenero_GenerosId",
                table: "FilmeGenero",
                column: "GenerosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmeGenero");

            migrationBuilder.DropTable(
                name: "Genero");

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Filmes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
