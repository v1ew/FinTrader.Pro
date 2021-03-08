using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class AddedCoupons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Isin = table.Column<string>(nullable: true),
                    CouponDate = table.Column<DateTime>(nullable: true),
                    RecordDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    InitialFaceValue = table.Column<double>(nullable: true),
                    FaceValue = table.Column<double>(nullable: true),
                    FaceUnit = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: true),
                    ValuePrc = table.Column<double>(nullable: true),
                    ValueRub = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.CouponId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
