using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GesamtNettoUmgerechnetErhalten",
                table: "DividendeErhalten",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GesamtNettoUmgerechnetErmittelt",
                table: "DividendeErhalten",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RundungArt",
                table: "DividendeErhalten",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GesamtNettoUmgerechnetErhalten",
                table: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "GesamtNettoUmgerechnetErmittelt",
                table: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "RundungArt",
                table: "DividendeErhalten");
        }
    }
}
