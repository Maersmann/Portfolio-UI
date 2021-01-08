using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Migrations
{
#pragma warning disable IDE1006 // Benennungsstile
    public partial class _4 : Migration
#pragma warning restore IDE1006 // Benennungsstile
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bezeichnung = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depot", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anzahl = table.Column<int>(nullable: false),
                    Preis = table.Column<double>(nullable: false),
                    Fremdkostenzuschlag = table.Column<double>(nullable: false),
                    Kaufdatum = table.Column<DateTime>(nullable: false),
                    Kaufart = table.Column<int>(nullable: false),
                    Orderart = table.Column<int>(nullable: false),
                    AktieID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Aktie_AktieID",
                        column: x => x.AktieID,
                        principalTable: "Aktie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepotAktien",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anzahl = table.Column<int>(nullable: false),
                    BuyIn = table.Column<double>(nullable: false),
                    AktieID = table.Column<int>(nullable: false),
                    DepotID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotAktien", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DepotAktien_Aktie_AktieID",
                        column: x => x.AktieID,
                        principalTable: "Aktie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepotAktien_Depot_DepotID",
                        column: x => x.DepotID,
                        principalTable: "Depot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepotAktien_AktieID",
                table: "DepotAktien",
                column: "AktieID");

            migrationBuilder.CreateIndex(
                name: "IX_DepotAktien_DepotID",
                table: "DepotAktien",
                column: "DepotID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_AktieID",
                table: "OrderHistory",
                column: "AktieID");

            migrationBuilder.InsertData("Depot", column: "Bezeichnung", value: "Standard");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepotAktien");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "Depot");
        }
    }
}
