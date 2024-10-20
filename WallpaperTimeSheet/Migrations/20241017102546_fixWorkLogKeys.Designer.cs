﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WallpaperTimeSheet;

#nullable disable

namespace WallpaperTimeSheet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241017102546_fixWorkLogKeys")]
    partial class fixWorkLogKeys
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("WallpaperTimeSheet.Models.WorkLog", b =>
                {
                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("WorkTaskId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DateTime");

                    b.HasIndex("WorkTaskId");

                    b.ToTable("WorkLogs");
                });

            modelBuilder.Entity("WallpaperTimeSheet.Models.WorkTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WorkTasks");
                });

            modelBuilder.Entity("WallpaperTimeSheet.Models.WorkLog", b =>
                {
                    b.HasOne("WallpaperTimeSheet.Models.WorkTask", "WorkTask")
                        .WithMany()
                        .HasForeignKey("WorkTaskId");

                    b.Navigation("WorkTask");
                });
#pragma warning restore 612, 618
        }
    }
}