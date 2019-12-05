﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSMB.SQL;

namespace SSMB.SQL.Migrations
{
    [DbContext(typeof(SsmbDbContext))]
    [Migration("20191205194707_BatchId")]
    partial class BatchId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0-preview3.19554.8");

            modelBuilder.Entity("SSMB.Domain.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnName("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<long>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("SSMB.Domain.AlertCondition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AlertId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Field")
                        .HasColumnType("INTEGER")
                        .IsUnicode(false);

                    b.Property<byte>("Operator")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Value")
                        .HasColumnName("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AlertId");

                    b.ToTable("AlertConditions");
                });

            modelBuilder.Entity("SSMB.Domain.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .IsUnicode(false);

                    b.Property<long>("ScrapValue")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(-1L);

                    b.Property<int>("Space")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StructuredDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .IsUnicode(false);

                    b.Property<long>("Weight")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SSMB.Domain.LtsOrderEntry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BatchId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderBatchBatchId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderBatchBatchId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .IsUnicode(false);

                    b.Property<long>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("EntryId");

                    b.HasIndex("BatchId");

                    b.HasIndex("OrderBatchBatchId");

                    b.HasIndex("OrderBatchBatchId1");

                    b.ToTable("LtsOrder");
                });

            modelBuilder.Entity("SSMB.Domain.OrderBatch", b =>
                {
                    b.Property<int>("BatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BatchId");

                    b.HasIndex("ItemId");

                    b.ToTable("OrderBatch");
                });

            modelBuilder.Entity("SSMB.Domain.OrderEntry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BaseName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(70);

                    b.Property<int>("BatchId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GalaxyName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(70);

                    b.Property<bool>("IsUserBase")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .IsUnicode(false);

                    b.Property<long>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("EntryId");

                    b.HasIndex("BatchId");

                    b.ToTable("OrderEntry");
                });

            modelBuilder.Entity("SSMB.Domain.Alert", b =>
                {
                    b.HasOne("SSMB.Domain.Item", "Item")
                        .WithMany("Alerts")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSMB.Domain.AlertCondition", b =>
                {
                    b.HasOne("SSMB.Domain.Alert", "Alert")
                        .WithMany("Conditions")
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSMB.Domain.LtsOrderEntry", b =>
                {
                    b.HasOne("SSMB.Domain.OrderBatch", "Batch")
                        .WithMany("LtsEntries")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSMB.Domain.OrderBatch", null)
                        .WithMany("LtsPurchaseEntries")
                        .HasForeignKey("OrderBatchBatchId");

                    b.HasOne("SSMB.Domain.OrderBatch", null)
                        .WithMany("LtsSaleEntries")
                        .HasForeignKey("OrderBatchBatchId1");
                });

            modelBuilder.Entity("SSMB.Domain.OrderBatch", b =>
                {
                    b.HasOne("SSMB.Domain.Item", "Item")
                        .WithMany("Orders")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSMB.Domain.OrderEntry", b =>
                {
                    b.HasOne("SSMB.Domain.OrderBatch", "Batch")
                        .WithMany("Entries")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
