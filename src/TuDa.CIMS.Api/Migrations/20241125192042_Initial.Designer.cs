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
    [Migration("20241125192042_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Hazard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChemicalId")
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChemicalId");

                    b.ToTable("Hazards");
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

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Chemical", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.AssetItem");

                    b.Property<string>("Cas")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Chemical");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Consumable", b =>
                {
                    b.HasBaseType("TuDa.CIMS.Shared.Entities.AssetItem");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Consumable");
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

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Hazard", b =>
                {
                    b.HasOne("TuDa.CIMS.Shared.Entities.Chemical", null)
                        .WithMany("Hazards")
                        .HasForeignKey("ChemicalId");
                });

            modelBuilder.Entity("TuDa.CIMS.Shared.Entities.Chemical", b =>
                {
                    b.Navigation("Hazards");
                });
#pragma warning restore 612, 618
        }
    }
}
