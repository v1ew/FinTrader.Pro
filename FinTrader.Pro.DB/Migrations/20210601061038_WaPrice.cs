using Microsoft.EntityFrameworkCore.Migrations;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class WaPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WaPrice",
                table: "Bonds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaPrice",
                table: "Bonds");
        }
    }
}
