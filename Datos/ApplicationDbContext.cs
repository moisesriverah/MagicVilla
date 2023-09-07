using MagicVillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Datos
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa..",
                    ImagenUrl = "",
                    Ocupantes = 12,
                    MetrosCuadrados = 85,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                      new Villa()
                      {
                          Id = 2,
                          Nombre = "Premium Vista a la Piscina ",
                          Detalle = "Detalle de la villa premium..",
                          ImagenUrl = "",
                          Ocupantes = 17,
                          MetrosCuadrados = 125,
                          Tarifa = 350,
                          Amenidad = "",
                          FechaCreacion = DateTime.Now,
                          FechaActualizacion = DateTime.Now
                      }
                );

        }
    }
}
