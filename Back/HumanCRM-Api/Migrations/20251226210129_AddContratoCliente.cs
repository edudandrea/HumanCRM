using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanCRM_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddContratoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContratosClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientesId = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeArquivo = table.Column<string>(type: "TEXT", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "TEXT", nullable: false),
                    TipoArquivo = table.Column<string>(type: "TEXT", nullable: false),
                    DataUpload = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratosClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratosClientes_Clientes_ClientesId",
                        column: x => x.ClientesId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratosClientes_ClientesId",
                table: "ContratosClientes",
                column: "ClientesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratosClientes");
        }
    }
}
