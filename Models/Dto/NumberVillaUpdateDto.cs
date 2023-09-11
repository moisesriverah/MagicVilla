using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVillaAPI.Models.Dto
{
    public class NumberVillaUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }

    }
}
