using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Migrations
{
    public partial class VW7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Anzahl",
                table: "OrderHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Bestand",
                table: "DividendeErhalten",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Anzahl",
                table: "DepotAktien",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Anzahl",
                table: "OrderHistory",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Bestand",
                table: "DividendeErhalten",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Anzahl",
                table: "DepotAktien",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
