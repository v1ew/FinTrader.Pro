﻿using System;
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
                    ShortName = table.Column<string>(nullable: true),
                    PrevWaPrice = table.Column<double>(nullable: true),
                    YieldAtPrevWaPrice = table.Column<double>(nullable: true),
                    CouponValue = table.Column<double>(nullable: true),
                    NextCoupon = table.Column<DateTime>(nullable: true),
                    AccruedInt = table.Column<double>(nullable: true),
                    PrevPrice = table.Column<double>(nullable: true),
                    LotSize = table.Column<int>(nullable: true),
                    FaceValue = table.Column<double>(nullable: true),
                    BoardName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    MatDate = table.Column<DateTime>(nullable: true),
                    Decimals = table.Column<int>(nullable: true),
                    CouponPeriod = table.Column<int>(nullable: true),
                    IssueSize = table.Column<long>(nullable: true),
                    PrevLegalClosePrice = table.Column<double>(nullable: true),
                    PrevAdmittedQuote = table.Column<double>(nullable: true),
                    PrevDate = table.Column<DateTime>(nullable: true),
                    SecName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    InstrId = table.Column<string>(nullable: true),
                    SectorId = table.Column<string>(nullable: true),
                    MinStep = table.Column<double>(nullable: true),
                    FaceUnit = table.Column<string>(nullable: true),
                    BuyBackPrice = table.Column<double>(nullable: true),
                    BuyBackDate = table.Column<DateTime>(nullable: true),
                    IsIn = table.Column<string>(nullable: true),
                    LatName = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<string>(nullable: true),
                    IssueSizePlaced = table.Column<long>(nullable: true),
                    ListLevel = table.Column<int>(nullable: true),
                    SecType = table.Column<string>(nullable: true),
                    CouponPercent = table.Column<double>(nullable: true),
                    OfferDate = table.Column<DateTime>(nullable: true),
                    SettleDate = table.Column<DateTime>(nullable: true),
                    LotValue = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonds", x => x.SecId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonds");
        }
    }
}
