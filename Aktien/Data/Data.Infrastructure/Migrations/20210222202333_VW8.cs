using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Infrastructure.Migrations
{
    public partial class VW8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RundungArt",
                table: "Dividende",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RundungArt",
                table: "Dividende");
        }
    }
}
