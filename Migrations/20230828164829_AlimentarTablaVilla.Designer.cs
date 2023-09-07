﻿// <auto-generated />
using System;
using MagicVillaAPI.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230828164829_AlimentarTablaVilla")]
    partial class AlimentarTablaVilla
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVillaAPI.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MetrosCuadrados")
                        .HasColumnType("float");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "Detalle de la Villa..",
                            FechaActualizacion = new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1926),
                            FechaCreacion = new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1884),
                            ImagenUrl = "",
                            MetrosCuadrados = 85.0,
                            Nombre = "Villa Real",
                            Ocupantes = 12,
                            Tarifa = 200.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "Detalle de la villa premium..",
                            FechaActualizacion = new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1931),
                            FechaCreacion = new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1930),
                            ImagenUrl = "",
                            MetrosCuadrados = 125.0,
                            Nombre = "Premium Vista a la Piscina ",
                            Ocupantes = 17,
                            Tarifa = 350.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
