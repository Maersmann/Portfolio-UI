using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class Issue54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Datum",
                table: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "Datum",
                table: "Dividende");

            migrationBuilder.AddColumn<DateTime>(
                name: "Exdatum",
                table: "Dividende",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Zahldatum",
                table: "Dividende",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exdatum",
                table: "Dividende");

            migrationBuilder.DropColumn(
                name: "Zahldatum",
                table: "Dividende");

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "DividendeErhalten",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "Dividende",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
