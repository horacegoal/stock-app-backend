﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace stockAppApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230725093734_relationStockTransaction")]
    partial class relationStockTransaction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("stockAppApi.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("Date")
                        .HasDefaultValueSql("datetime('now')");

                    b.Property<double?>("MarketCap")
                        .HasColumnType("REAL")
                        .HasColumnName("MarketCap");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<double?>("PERatio")
                        .HasColumnType("REAL")
                        .HasColumnName("PERatio");

                    b.Property<double?>("PercentageChange")
                        .HasColumnType("REAL")
                        .HasColumnName("PercentageChange");

                    b.Property<double?>("Price")
                        .HasColumnType("REAL")
                        .HasColumnName("Price");

                    b.Property<double?>("ShareFloat")
                        .HasColumnType("REAL")
                        .HasColumnName("ShareFloat");

                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT")
                        .HasColumnName("Symbol");

                    b.Property<double?>("Volume")
                        .HasColumnType("REAL")
                        .HasColumnName("Volume");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("stockAppApi.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("DateCreated")
                        .HasDefaultValueSql("datetime('now')");

                    b.Property<double>("Price")
                        .HasColumnType("REAL")
                        .HasColumnName("Price");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Quantity");

                    b.Property<int?>("StockId")
                        .IsRequired()
                        .HasColumnType("INTEGER")
                        .HasColumnName("StockId");

                    b.Property<int?>("Type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("stockAppApi.Entities.Transaction", b =>
                {
                    b.HasOne("stockAppApi.Entities.Stock", "Stock")
                        .WithMany("Transactions")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("stockAppApi.Entities.Stock", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
