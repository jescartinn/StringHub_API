using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringHub.Models
{
    public class OrdenEncordado
    {
        [Key]
        public int OrdenId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int RaquetaId { get; set; }

        [Required]
        public int ServicioId { get; set; }

        public int? CuerdaId { get; set; }

        [Required, Column(TypeName = "decimal(4,1)")]
        public decimal TensionVertical { get; set; }

        [Column(TypeName = "decimal(4,1)")]
        public decimal? TensionHorizontal { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; } = null!;

        public string? Comentarios { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaCompletado { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }

        public int? EncordadorId { get; set; }

        // Relaciones de navegación
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("RaquetaId")]
        public Raqueta? Raqueta { get; set; }

        [ForeignKey("ServicioId")]
        public Servicio? Servicio { get; set; }

        [ForeignKey("CuerdaId")]
        public Cuerda? Cuerda { get; set; }

        [ForeignKey("EncordadorId")]
        public Usuario? Encordador { get; set; }

        public ICollection<HistorialTension>? HistorialTensiones { get; set; }
    }
}