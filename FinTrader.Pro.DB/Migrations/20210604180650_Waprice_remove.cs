using Microsoft.EntityFrameworkCore.Migrations;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class Waprice_remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaPrice",
                table: "Bonds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WaPrice",
                table: "Bonds",
                type: "double precision",
                nullable: true);
        }
    }
}
