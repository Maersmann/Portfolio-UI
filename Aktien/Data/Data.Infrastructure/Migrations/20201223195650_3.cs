using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Migrations
{
#pragma warning disable IDE1006 // Benennungsstile
    public partial class _3 : Migration
#pragma warning restore IDE1006 // Benennungsstile
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dividende",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    Betrag = table.Column<double>(nullable: false),
                    AktieID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dividende", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dividende_Aktie_AktieID",
                        column: x => x.AktieID,
                        principalTable: "Aktie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dividende_AktieID",
                table: "Dividende",
                column: "AktieID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dividende");
        }
    }
}
