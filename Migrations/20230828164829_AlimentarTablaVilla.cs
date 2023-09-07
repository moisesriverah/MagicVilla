using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Villa..", new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1926), new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1884), "", 85.0, "Villa Real", 12, 200.0 },
                    { 2, "", "Detalle de la villa premium..", new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1931), new DateTime(2023, 8, 28, 12, 48, 29, 709, DateTimeKind.Local).AddTicks(1930), "", 125.0, "Premium Vista a la Piscina ", 17, 350.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
