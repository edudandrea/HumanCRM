using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanCRM_Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterTelefoneToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Observacoes",
                table: "Clientes",
                newName: "Obs");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Clientes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Clientes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Obs",
                table: "Clientes",
                newName: "Observacoes");

            migrationBuilder.AlterColumn<int>(
                name: "Telefone",
                table: "Clientes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
