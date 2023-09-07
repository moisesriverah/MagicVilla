using MagicVillaAPI.Models.Dto;

namespace MagicVillaAPI.Datos
{
    public  static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>()
        {
                new VillaDto {Id=1, Nombre="Vista a la piscina" },
                new VillaDto {Id=2, Nombre="Vista a la playa" },
                new VillaDto {Id=3, Nombre="Vista al parque" }
        };
    }
}
