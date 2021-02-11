using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GesamtAusgaben",
                table: "Depot",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GesamtEinahmen",
                table: "Depot",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Einnahme",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Betrag = table.Column<double>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Art = table.Column<int>(nullable: false),
                    HerkunftID = table.Column<int>(nullable: true),
                    DepotID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Einnahme", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Einnahme_Depot_DepotID",
                        column: x => x.DepotID,
                        principalTable: "Depot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Einnahme_DepotID",
                table: "Einnahme",
                column: "DepotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Einnahme");

            migrationBuilder.DropColumn(
                name: "GesamtAusgaben",
                table: "Depot");

            migrationBuilder.DropColumn(
                name: "GesamtEinahmen",
                table: "Depot");
        }
    }
}
