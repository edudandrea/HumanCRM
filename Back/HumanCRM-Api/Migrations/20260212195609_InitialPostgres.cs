using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HumanCRM_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClienteDocumentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    NomeArquivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Tamanho = table.Column<long>(type: "bigint", nullable: false),
                    Arquivo = table.Column<byte[]>(type: "bytea", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteDocumentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoPessoa = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RG = table.Column<string>(type: "text", nullable: true),
                    CpfCnpj = table.Column<string>(type: "text", nullable: true),
                    Cep = table.Column<int>(type: "integer", nullable: false),
                    Rua = table.Column<string>(type: "text", nullable: true),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: true),
                    Cidade = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    DDD = table.Column<int>(type: "integer", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Celular = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    RedeSocial = table.Column<string>(type: "text", nullable: true),
                    ResponsavelContato = table.Column<string>(type: "text", nullable: true),
                    OrigemContato = table.Column<string>(type: "text", nullable: true),
                    Obs = table.Column<string>(type: "text", nullable: true),
                    RazaoSocial = table.Column<string>(type: "text", nullable: true),
                    IE = table.Column<int>(type: "integer", nullable: false),
                    IM = table.Column<int>(type: "integer", nullable: false),
                    OrgaoExpedidor = table.Column<string>(type: "text", nullable: true),
                    Sexo = table.Column<int>(type: "integer", nullable: false),
                    EstadoCivil = table.Column<int>(type: "integer", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFuncacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataContato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    AgendamentoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    DataAgendamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ResponsavelAgendamento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.AgendamentoId);
                });

            migrationBuilder.CreateTable(
                name: "ContratosClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    ClientesId = table.Column<int>(type: "integer", nullable: true),
                    NomeArquivo = table.Column<string>(type: "text", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "text", nullable: false),
                    TipoArquivo = table.Column<string>(type: "text", nullable: false),
                    DataUpload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ProspeccoesClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    ContatoProspeccao = table.Column<string>(type: "text", nullable: true),
                    Etapa = table.Column<string>(type: "text", nullable: true),
                    Probabilidade = table.Column<int>(type: "integer", nullable: true),
                    OrigemContato = table.Column<string>(type: "text", nullable: true),
                    InteressePrincipal = table.Column<string>(type: "text", nullable: true),
                    Necessidade = table.Column<string>(type: "text", nullable: true),
                    DataProximoContato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Canal = table.Column<string>(type: "text", nullable: true),
                    Responsavel = table.Column<string>(type: "text", nullable: true),
                    Observacoes = table.Column<string>(type: "text", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "IX_ClienteDocumentos_ClienteId",
                table: "ClienteDocumentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosClientes_ClientesId",
                table: "ContratosClientes",
                column: "ClientesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProspeccoesClientes_ClienteId",
                table: "ProspeccoesClientes",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteDocumentos");

            migrationBuilder.DropTable(
                name: "ContratosClientes");

            migrationBuilder.DropTable(
                name: "ProspeccoesClientes");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
