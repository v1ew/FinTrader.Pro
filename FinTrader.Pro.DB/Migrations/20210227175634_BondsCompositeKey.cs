using Microsoft.EntityFrameworkCore.Migrations;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class BondsCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bonds",
                table: "Bonds");

            migrationBuilder.AlterColumn<string>(
                name: "BoardId",
                table: "Bonds",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bonds",
                table: "Bonds",
                columns: new[] { "SecId", "BoardId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bonds",
                table: "Bonds");

            migrationBuilder.AlterColumn<string>(
                name: "BoardId",
                table: "Bonds",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bonds",
                table: "Bonds",
                column: "SecId");
        }
    }
}
