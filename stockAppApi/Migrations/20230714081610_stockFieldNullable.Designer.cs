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
    [Migration("20230714081610_stockFieldNullable")]
    partial class stockFieldNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("stockAppApi.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<decimal?>("Close")
                        .HasColumnType("TEXT")
                        .HasColumnName("Close");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT")
                        .HasColumnName("Date");

                    b.Property<decimal?>("High")
                        .HasColumnType("TEXT")
                        .HasColumnName("High");

                    b.Property<decimal?>("Low")
                        .HasColumnType("TEXT")
                        .HasColumnName("Low");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<decimal?>("Open")
                        .HasColumnType("TEXT")
                        .HasColumnName("Open");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Symbol");

                    b.Property<decimal?>("Volume")
                        .HasColumnType("TEXT")
                        .HasColumnName("Volume");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}
