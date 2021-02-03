using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Aktie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WKN",
                table: "Aktie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WKN",
                table: "Aktie");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Aktie",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

        }
    }
}
