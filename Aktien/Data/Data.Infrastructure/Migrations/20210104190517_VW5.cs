using Microsoft.EntityFrameworkCore.Migrations;

namespace Aktien.Data.Migrations
{
    public partial class VW5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kaufart",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "Orderart",
                table: "OrderHistory");

            migrationBuilder.AlterColumn<double>(
                name: "Fremdkostenzuschlag",
                table: "OrderHistory",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<int>(
                name: "BuySell",
                table: "OrderHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KaufartTyp",
                table: "OrderHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderartTyp",
                table: "OrderHistory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuySell",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "KaufartTyp",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "OrderartTyp",
                table: "OrderHistory");

            migrationBuilder.AlterColumn<double>(
                name: "Fremdkostenzuschlag",
                table: "OrderHistory",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kaufart",
                table: "OrderHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Orderart",
                table: "OrderHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
