﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace stockAppApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("stockAppApi.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int?>("AverageDailyVolume10Day")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("stockAppApi.Entities.StockHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<double>("ClosePrice")
                        .HasColumnType("REAL")
                        .HasColumnName("ClosePrice");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT")
                        .HasColumnName("Date");

                    b.Property<int>("StockId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("StockId");

                    b.Property<int>("Volumn")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Volume");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockHistory");
                });

            modelBuilder.Entity("stockAppApi.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

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

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("DateCreated");

                    b.Property<int?>("Type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("stockAppApi.Entities.StockHistory", b =>
                {
                    b.HasOne("stockAppApi.Entities.Stock", "Stock")
                        .WithMany("StockHistories")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
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
                    b.Navigation("StockHistories");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
