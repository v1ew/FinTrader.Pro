﻿// <auto-generated />
using System;
using FinTrader.Pro.DB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FinTrader.Pro.DB.Migrations
{
    [DbContext(typeof(FinTraderDataContext))]
    partial class FinTraderDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<double?>("AccruedInt")
                        .HasColumnType("double precision");

                    b.Property<string>("BoardId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BuyBackDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("BuyBackPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

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

                    b.Property<bool>("Discarded")
                        .HasColumnType("boolean");

                    b.Property<double?>("Duration")
                        .HasColumnType("double precision");

                    b.Property<int?>("EmitterId")
                        .HasColumnType("integer");

                    b.Property<string>("FaceUnit")
                        .HasColumnType("text");

                    b.Property<double?>("FaceValue")
                        .HasColumnType("double precision");

                    b.Property<string>("Isin")
                        .HasColumnType("text");

                    b.Property<long?>("IssueSize")
                        .HasColumnType("bigint");

                    b.Property<int?>("LotSize")
                        .HasColumnType("integer");

                    b.Property<double?>("LotValue")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("MatDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("ModifiedDuration")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("NextCoupon")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("OfferDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("PrevDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("PrevWaPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("SecName")
                        .HasColumnType("text");

                    b.Property<string>("SecType")
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("ValueAvg")
                        .HasColumnType("double precision");

                    b.Property<double?>("Yield")
                        .HasColumnType("double precision");

                    b.Property<double?>("YieldAtPrevWaPrice")
                        .HasColumnType("double precision");

                    b.HasKey("SecId");

                    b.HasIndex("Isin");

                    b.ToTable("Bonds");
                });

            modelBuilder.Entity("FinTrader.Pro.DB.Models.Config", b =>
                {
                    b.Property<int>("BondsCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(6);

                    b.Property<int>("MaxYield")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(15);

                    b.ToTable("Config");
                });

            modelBuilder.Entity("FinTrader.Pro.DB.Models.Coupon", b =>
                {
                    b.Property<int>("CouponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CouponDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FaceUnit")
                        .HasColumnType("text");

                    b.Property<double?>("FaceValue")
                        .HasColumnType("double precision");

                    b.Property<double?>("InitialFaceValue")
                        .HasColumnType("double precision");

                    b.Property<string>("Isin")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("Value")
                        .HasColumnType("double precision");

                    b.Property<double?>("ValuePrc")
                        .HasColumnType("double precision");

                    b.Property<double?>("ValueRub")
                        .HasColumnType("double precision");

                    b.HasKey("CouponId");

                    b.HasIndex("Isin");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("FinTrader.Pro.DB.Models.TradeDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("TradeDates");
                });
#pragma warning restore 612, 618
        }
    }
}
