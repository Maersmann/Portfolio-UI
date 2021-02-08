using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW1 : Migration
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
                name: "Wertpapier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    ISIN = table.Column<string>(nullable: true),
                    WKN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wertpapier", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DepotWertpapier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anzahl = table.Column<double>(nullable: false),
                    BuyIn = table.Column<double>(nullable: false),
                    WertpapierID = table.Column<int>(nullable: false),
                    DepotID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotWertpapier", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DepotWertpapier_Depot_DepotID",
                        column: x => x.DepotID,
                        principalTable: "Depot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepotWertpapier_Wertpapier_WertpapierID",
                        column: x => x.WertpapierID,
                        principalTable: "Wertpapier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dividende",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    Betrag = table.Column<double>(nullable: false),
                    WertpapierID = table.Column<int>(nullable: false),
                    Waehrung = table.Column<int>(nullable: false),
                    BetragUmgerechnet = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dividende", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dividende_Wertpapier_WertpapierID",
                        column: x => x.WertpapierID,
                        principalTable: "Wertpapier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anzahl = table.Column<double>(nullable: false),
                    Preis = table.Column<double>(nullable: false),
                    Fremdkostenzuschlag = table.Column<double>(nullable: true),
                    Orderdatum = table.Column<DateTime>(nullable: false),
                    WertpapierID = table.Column<int>(nullable: false),
                    KaufartTyp = table.Column<int>(nullable: false),
                    OrderartTyp = table.Column<int>(nullable: false),
                    BuySell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Wertpapier_WertpapierID",
                        column: x => x.WertpapierID,
                        principalTable: "Wertpapier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    Bestand = table.Column<double>(nullable: false),
                    DividendeID = table.Column<int>(nullable: false),
                    WertpapierID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendeErhalten", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DividendeErhalten_Dividende_DividendeID",
                        column: x => x.DividendeID,
                        principalTable: "Dividende",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DividendeErhalten_Wertpapier_WertpapierID",
                        column: x => x.WertpapierID,
                        principalTable: "Wertpapier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepotWertpapier_DepotID",
                table: "DepotWertpapier",
                column: "DepotID");

            migrationBuilder.CreateIndex(
                name: "IX_DepotWertpapier_WertpapierID",
                table: "DepotWertpapier",
                column: "WertpapierID");

            migrationBuilder.CreateIndex(
                name: "IX_Dividende_WertpapierID",
                table: "Dividende",
                column: "WertpapierID");

            migrationBuilder.CreateIndex(
                name: "IX_DividendeErhalten_DividendeID",
                table: "DividendeErhalten",
                column: "DividendeID");

            migrationBuilder.CreateIndex(
                name: "IX_DividendeErhalten_WertpapierID",
                table: "DividendeErhalten",
                column: "WertpapierID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_WertpapierID",
                table: "OrderHistory",
                column: "WertpapierID");

            migrationBuilder.InsertData("Depot", column: "Bezeichnung", value: "Standard");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepotWertpapier");

            migrationBuilder.DropTable(
                name: "DividendeErhalten");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "Depot");

            migrationBuilder.DropTable(
                name: "Dividende");

            migrationBuilder.DropTable(
                name: "Wertpapier");
        }
    }
}
