using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class NextCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextCoupon",
                table: "Bonds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextCoupon",
                table: "Bonds");
        }
    }
}
