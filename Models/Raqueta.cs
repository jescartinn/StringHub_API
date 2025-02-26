using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringHub.Models
{
    public class Raqueta
    {
        [Key]
        public int RaquetaId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required, MaxLength(50)]
        public string Marca { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Modelo { get; set; } = null!;
        
        [MaxLength(50)]
        public string? NumeroSerie { get; set; }
        
        public string? Descripcion { get; set; }
        
        public DateTime FechaCreacion { get; set; }

        // Relaciones de navegación
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        
        public ICollection<HistorialTension>? HistorialTensiones { get; set; }
        public ICollection<OrdenEncordado>? Ordenes { get; set; }
    }
}