using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Migrations
{
    public partial class VW6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BetragUmgerechnet",
                table: "Dividende",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Waehrung",
                table: "Dividende",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DividendeErhalten",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    Quellensteuer = table.Column<double>(nullable: true),
                    Umrechnungskurs = table.Column<double>(nullable: true),
                    GesamtNetto = table.Column<double>(nullable: false),
                    GesamtBrutto = table.Column<double>(nullable: false),
                    Bestand = table.Column<int>(nullable: false),
                    AktieID = table.Column<int>(nullable: false),
                    DividendeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendeErhalten", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DividendeErhalten_Aktie_AktieID",
                        column: x => x.AktieID,
                        principalTable: "Aktie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DividendeErhalten_Dividende_DividendeID",
                        column: x => x.DividendeID,
                        principalTable: "Dividende",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DividendeErhalten_AktieID",
                table: "DividendeErhalten",
                column: "AktieID");

            migrationBuilder.CreateIndex(
                name: "IX_DividendeErhalten_DividendeID",
                table: "DividendeErhalten",
                column: "DividendeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendeErhalten");

            migrationBuilder.DropColumn(
                name: "BetragUmgerechnet",
                table: "Dividende");

            migrationBuilder.DropColumn(
                name: "Waehrung",
                table: "Dividende");
        }
    }
}
