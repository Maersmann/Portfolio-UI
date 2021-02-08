using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WertpapierTyp",
                table: "Wertpapier",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WertpapierTyp",
                table: "Wertpapier");
        }
    }
}
