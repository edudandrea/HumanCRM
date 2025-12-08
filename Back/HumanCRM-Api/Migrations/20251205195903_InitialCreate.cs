using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanCRM_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TipoPessoa = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CpfCnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    Rua = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    RedeSocial = table.Column<string>(type: "TEXT", nullable: true),
                    ResponsavelContato = table.Column<string>(type: "TEXT", nullable: true),
                    OrigemContato = table.Column<string>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProspeccoesClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Etapa = table.Column<string>(type: "TEXT", nullable: true),
                    Probabilidade = table.Column<int>(type: "INTEGER", nullable: true),
                    OrigemContato = table.Column<string>(type: "TEXT", nullable: true),
                    InteressePrincipal = table.Column<string>(type: "TEXT", nullable: true),
                    Necessidade = table.Column<string>(type: "TEXT", nullable: true),
                    DataProximoContato = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Canal = table.Column<string>(type: "TEXT", nullable: true),
                    Responsavel = table.Column<string>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspeccoesClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProspeccoesClientes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProspeccoesClientes_ClienteId",
                table: "ProspeccoesClientes",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProspeccoesClientes");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
