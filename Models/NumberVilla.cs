using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillaAPI.Models
{
    public class NumberVilla
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        [ForeignKey("VillaId")]
        public Villa villa { get; set; }
        public string DetalleEspecial { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
