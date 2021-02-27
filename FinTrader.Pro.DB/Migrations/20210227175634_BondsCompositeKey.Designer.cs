﻿// <auto-generated />
using System;
using FinTrader.Pro.DB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FinTrader.Pro.DB.Migrations
{
    [DbContext(typeof(FinTraderDataContext))]
    [Migration("20210227175634_BondsCompositeKey")]
    partial class BondsCompositeKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("FinTrader.Pro.DB.Models.Bond", b =>
                {
                    b.Property<string>("SecId")
                        .HasColumnType("text");

                    b.Property<string>("BoardId")
                        .HasColumnType("text");

                    b.Property<double?>("AccruedInt")
                        .HasColumnType("double precision");

                    b.Property<string>("BoardName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BuyBackDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("BuyBackPrice")
                        .HasColumnType("double precision");

                    b.Property<double?>("CouponPercent")
                        .HasColumnType("double precision");

                    b.Property<int?>("CouponPeriod")
                        .HasColumnType("integer");

                    b.Property<double?>("CouponValue")
                        .HasColumnType("double precision");

                    b.Property<string>("CurrencyId")
                        .HasColumnType("text");

                    b.Property<int?>("Decimals")
                        .HasColumnType("integer");

                    b.Property<string>("FaceUnit")
                        .HasColumnType("text");

                    b.Property<double?>("FaceValue")
                        .HasColumnType("double precision");

                    b.Property<string>("InstrId")
                        .HasColumnType("text");

                    b.Property<string>("IsIn")
                        .HasColumnType("text");

                    b.Property<long?>("IssueSize")
                        .HasColumnType("bigint");

                    b.Property<long?>("IssueSizePlaced")
                        .HasColumnType("bigint");

                    b.Property<string>("LatName")
                        .HasColumnType("text");

                    b.Property<int?>("ListLevel")
                        .HasColumnType("integer");

                    b.Property<int?>("LotSize")
                        .HasColumnType("integer");

                    b.Property<double?>("LotValue")
                        .HasColumnType("double precision");

                    b.Property<string>("MarketCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("MatDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("MinStep")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("NextCoupon")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("OfferDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("PrevAdmittedQuote")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("PrevDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("PrevLegalClosePrice")
                        .HasColumnType("double precision");

                    b.Property<double?>("PrevPrice")
                        .HasColumnType("double precision");

                    b.Property<double?>("PrevWaPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("RegNumber")
                        .HasColumnType("text");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<string>("SecName")
                        .HasColumnType("text");

                    b.Property<string>("SecType")
                        .HasColumnType("text");

                    b.Property<string>("SectorId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SettleDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ShortName")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<double?>("YieldAtPrevWaPrice")
                        .HasColumnType("double precision");

                    b.HasKey("SecId", "BoardId");

                    b.ToTable("Bonds");
                });
#pragma warning restore 612, 618
        }
    }
}
