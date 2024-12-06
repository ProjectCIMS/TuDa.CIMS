﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TuDa.CIMS.Api.Database;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    [DbContext(typeof(CIMSDbContext))]
    [Migration("20241206182604_AddMissingEntitiesAndImproveStructure")]
    partial class AddMissingEntitiesAndImproveStructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HazardSubstance", b =>
                {
                    b.Property<Guid>("HazardsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubstancesId")
                        .HasColumnType("uuid");

                    b.HasKey("HazardsId", "SubstancesId");

                    b.HasIndex("SubstancesId");

                    b.ToTable("HazardSubstance");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.AssetItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("ItemNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<string>("Shop")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("AssetItems");

                    b.HasDiscriminator().HasValue("AssetItem");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.ConsumableTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AmountChange")
                        .HasColumnType("integer");

                    b.Property<Guid>("ConsumableId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TransactionReason")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConsumableId");

                    b.ToTable("ConsumableTransactions");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Hazard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Hazards");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Person");

                    b.HasDiscriminator().HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("Signature")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<Guid>("WorkingGroupId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("WorkingGroupId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.PurchaseEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid>("AssetItemId")
                        .HasColumnType("uuid");

                    b.Property<double>("PricePerItem")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("PurchaseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AssetItemId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("PurchaseEntries");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.WorkingGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProfessorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProfessorId")
                        .IsUnique();

                    b.ToTable("WorkingGroups");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Consumable", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.AssetItem");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Consumable");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Substance", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.AssetItem");

                    b.Property<string>("Cas")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PriceUnit")
                        .HasColumnType("integer");

                    b.Property<double>("Purity")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("Substance");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Professor", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.Person");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Student", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.Person");

                    b.Property<Guid>("WorkingGroupId")
                        .HasColumnType("uuid");

                    b.HasIndex("WorkingGroupId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Chemical", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.Substance");

                    b.Property<double>("BindingSize")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("Chemical");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Solvent", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.Chemical");

                    b.HasDiscriminator().HasValue("Solvent");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.GasCylinder", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.Solvent");

                    b.Property<double>("Pressure")
                        .HasColumnType("double precision");

                    b.Property<double>("Volume")
                        .HasColumnType("double precision");

                    b.HasDiscriminator().HasValue("GasCylinder");
                });

            modelBuilder.Entity("HazardSubstance", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Hazard", null)
                        .WithMany()
                        .HasForeignKey("HazardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuDa.CIMS.Shared.Entities.Substance", null)
                        .WithMany()
                        .HasForeignKey("SubstancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.AssetItem", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.ConsumableTransaction", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Consumable", "Consumable")
                        .WithMany()
                        .HasForeignKey("ConsumableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumable");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Purchase", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Person", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuDa.CIMS.Shared.Entities.WorkingGroup", "WorkingGroup")
                        .WithMany("Purchases")
                        .HasForeignKey("WorkingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("WorkingGroup");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.PurchaseEntry", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.AssetItem", "AssetItem")
                        .WithMany()
                        .HasForeignKey("AssetItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuDa.CIMS.Shared.Entities.Purchase", null)
                        .WithMany("Entries")
                        .HasForeignKey("PurchaseId");

                    b.Navigation("AssetItem");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.WorkingGroup", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Professor", "Professor")
                        .WithOne("WorkingGroup")
                        .HasForeignKey("TuDa.CIMS.Shared.Entities.WorkingGroup", "ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Student", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.WorkingGroup", "WorkingGroup")
                        .WithMany("Students")
                        .HasForeignKey("WorkingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkingGroup");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Purchase", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.WorkingGroup", b =>
                {
                    b.Navigation("Purchases");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Professor", b =>
                {
                    b.Navigation("WorkingGroup")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
