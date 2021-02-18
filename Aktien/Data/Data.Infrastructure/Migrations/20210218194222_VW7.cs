using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GesamtNettoUmgerechnet",
                table: "DividendeErhalten",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GesamtNettoUmgerechnetUngerundet",
                table: "DividendeErhalten",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RundundArt",
                table: "DividendeErhalten",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GesamtNettoUmgerechnet",
                table: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "GesamtNettoUmgerechnetUngerundet",
                table: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "RundundArt",
                table: "DividendeErhalten");
        }
    }
}
