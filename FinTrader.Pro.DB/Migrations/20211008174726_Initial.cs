using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinTrader.Pro.DB.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonds",
                columns: table => new
                {
                    SecId = table.Column<string>(nullable: false),
                    BoardId = table.Column<string>(nullable: true),
                    Discarded = table.Column<bool>(nullable: false),
                    ShortName = table.Column<string>(nullable: true),
                    PrevWaPrice = table.Column<double>(nullable: true),
                    YieldAtPrevWaPrice = table.Column<double>(nullable: true),
                    NextCoupon = table.Column<DateTime>(nullable: true),
                    CouponValue = table.Column<double>(nullable: true),
                    AccruedInt = table.Column<double>(nullable: true),
                    LotSize = table.Column<int>(nullable: true),
                    FaceValue = table.Column<double>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    MatDate = table.Column<DateTime>(nullable: true),
                    Decimals = table.Column<int>(nullable: true),
                    CouponPeriod = table.Column<int>(nullable: true),
                    IssueSize = table.Column<long>(nullable: true),
                    SecName = table.Column<string>(nullable: true),
                    FaceUnit = table.Column<string>(nullable: true),
                    Isin = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<string>(nullable: true),
                    SecType = table.Column<string>(nullable: true),
                    CouponPercent = table.Column<double>(nullable: true),
                    OfferDate = table.Column<DateTime>(nullable: true),
                    LotValue = table.Column<double>(nullable: true),
                    EmitterId = table.Column<int>(nullable: true),
                    Duration = table.Column<double>(nullable: true),
                    ModifiedDuration = table.Column<double>(nullable: true),
                    Yield = table.Column<double>(nullable: true),
                    ValueAvg = table.Column<double>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonds", x => x.SecId);
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    BondsCount = table.Column<int>(nullable: false, defaultValue: 6),
                    MaxYield = table.Column<int>(nullable: false, defaultValue: 15)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.CreateTable(
                name: "TradeDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeDates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonds_Isin",
                table: "Bonds",
                column: "Isin");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Isin",
                table: "Coupons",
                column: "Isin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonds");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "TradeDates");
        }
    }
}
