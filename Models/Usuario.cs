using System.ComponentModel.DataAnnotations;

namespace StringHub.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        
        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [Required, MaxLength(255)]
        public string Contraseña { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Apellido { get; set; } = null!;
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [Required, MaxLength(20)]
        public string TipoUsuario { get; set; } = null!;
        
        public DateTime FechaCreacion { get; set; }
        
        public DateTime UltimaModificacion { get; set; }

        // Relaciones de navegación
        public ICollection<Raqueta>? Raquetas { get; set; }
        public ICollection<OrdenEncordado>? OrdenesComoCliente { get; set; }
        public ICollection<OrdenEncordado>? OrdenesProcesadas { get; set; }
        public ICollection<Disponibilidad>? Disponibilidades { get; set; }
    }
}