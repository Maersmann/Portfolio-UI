using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ausgabe",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Betrag = table.Column<double>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Art = table.Column<int>(nullable: false),
                    HerkunftID = table.Column<int>(nullable: true),
                    Beschreibung = table.Column<string>(nullable: true),
                    DepotID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ausgabe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ausgabe_Depot_DepotID",
                        column: x => x.DepotID,
                        principalTable: "Depot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ausgabe_DepotID",
                table: "Ausgabe",
                column: "DepotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ausgabe");
        }
    }
}
