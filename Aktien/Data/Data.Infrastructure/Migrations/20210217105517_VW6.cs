using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ETFInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WertpapierID = table.Column<int>(nullable: false),
                    Emittent = table.Column<int>(nullable: false),
                    ProfitTyp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETFInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ETFInfo_Wertpapier_WertpapierID",
                        column: x => x.WertpapierID,
                        principalTable: "Wertpapier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETFInfo_WertpapierID",
                table: "ETFInfo",
                column: "WertpapierID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETFInfo");
        }
    }
}
