using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanCRM_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteDocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClienteDocumentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomeArquivo = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Tamanho = table.Column<int>(type: "INTEGER", nullable: false),
                    Arquivo = table.Column<byte[]>(type: "BLOB", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteDocumentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteDocumentos_ClienteId",
                table: "ClienteDocumentos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteDocumentos");
        }
    }
}
