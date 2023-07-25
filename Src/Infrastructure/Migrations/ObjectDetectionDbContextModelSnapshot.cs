﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ObjectDetectionDbContext))]
    partial class ObjectDetectionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.AggregateModels.AccessAccountAggregate.AccessAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Domain.AggregateModels.OriginalFileAggregate.OriginalFile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("OriginalFiles", (string)null);
                });

            modelBuilder.Entity("Domain.AggregateModels.ProcessedFileAggregate.ProcessedFile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("ProcessedFiles", (string)null);
                });

            modelBuilder.Entity("ViewersToProcessedFiles", b =>
                {
                    b.Property<string>("ViewerId")
                        .HasColumnType("text");

                    b.Property<Guid>("ProcessedFileId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("ViewerId", "ProcessedFileId");

                    b.HasIndex("ProcessedFileId");

                    b.ToTable("ViewersToProcessedFiles");
                });

            modelBuilder.Entity("Domain.AggregateModels.OriginalFileAggregate.OriginalFile", b =>
                {
                    b.HasOne("Domain.AggregateModels.AccessAccountAggregate.AccessAccount", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.AggregateModels.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("OriginalFileId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Type")
                                .HasColumnType("integer");

                            b1.HasKey("OriginalFileId");

                            b1.ToTable("OriginalFiles");

                            b1.WithOwner()
                                .HasForeignKey("OriginalFileId");
                        });

                    b.OwnsOne("Domain.AggregateModels.StorageData", "StorageData", b1 =>
                        {
                            b1.Property<Guid>("OriginalFileId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Storage")
                                .HasColumnType("integer");

                            b1.Property<string>("Uri")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OriginalFileId");

                            b1.ToTable("OriginalFiles");

                            b1.WithOwner()
                                .HasForeignKey("OriginalFileId");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("StorageData")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.AggregateModels.ProcessedFileAggregate.ProcessedFile", b =>
                {
                    b.HasOne("Domain.AggregateModels.AccessAccountAggregate.AccessAccount", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.AggregateModels.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("ProcessedFileId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Type")
                                .HasColumnType("integer");

                            b1.HasKey("ProcessedFileId");

                            b1.ToTable("ProcessedFiles");

                            b1.WithOwner()
                                .HasForeignKey("ProcessedFileId");
                        });

                    b.OwnsOne("Domain.AggregateModels.StorageData", "StorageData", b1 =>
                        {
                            b1.Property<Guid>("ProcessedFileId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Storage")
                                .HasColumnType("integer");

                            b1.Property<string>("Uri")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ProcessedFileId");

                            b1.ToTable("ProcessedFiles");

                            b1.WithOwner()
                                .HasForeignKey("ProcessedFileId");
                        });

                    b.OwnsOne("Domain.AggregateModels.ServeData", "ServeData", b1 =>
                        {
                            b1.Property<Guid>("ProcessedFileId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ProcessedFileId");

                            b1.ToTable("ProcessedFiles");

                            b1.WithOwner()
                                .HasForeignKey("ProcessedFileId");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("ServeData")
                        .IsRequired();

                    b.Navigation("StorageData")
                        .IsRequired();
                });

            modelBuilder.Entity("ViewersToProcessedFiles", b =>
                {
                    b.HasOne("Domain.AggregateModels.ProcessedFileAggregate.ProcessedFile", null)
                        .WithMany()
                        .HasForeignKey("ProcessedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.AggregateModels.AccessAccountAggregate.AccessAccount", null)
                        .WithMany()
                        .HasForeignKey("ViewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
