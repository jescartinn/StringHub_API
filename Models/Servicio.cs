using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringHub.Models
{
    public class Servicio
    {
        [Key]
        public int ServicioId { get; set; }
        
        [Required, MaxLength(100)]
        public string NombreServicio { get; set; } = null!;
        
        public string? Descripcion { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioBase { get; set; }
        
        [Required]
        public int TiempoEstimado { get; set; }
        
        public bool Activo { get; set; }

        // Relaciones de navegación
        public ICollection<OrdenEncordado>? Ordenes { get; set; }
    }
}