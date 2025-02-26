using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringHub.Models
{
    public class HistorialTension
    {
        [Key]
        public int HistorialId { get; set; }

        [Required]
        public int RaquetaId { get; set; }

        [Required]
        public int OrdenId { get; set; }

        [Required, Column(TypeName = "decimal(4,1)")]
        public decimal TensionVertical { get; set; }

        [Column(TypeName = "decimal(4,1)")]
        public decimal? TensionHorizontal { get; set; }

        public int? CuerdaId { get; set; }

        public DateTime Fecha { get; set; }

        // Relaciones de navegación
        [ForeignKey("RaquetaId")]
        public Raqueta? Raqueta { get; set; }

        [ForeignKey("OrdenId")]
        public OrdenEncordado? Orden { get; set; }

        [ForeignKey("CuerdaId")]
        public Cuerda? Cuerda { get; set; }
    }
}