using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class UsuarioCreateDto
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [Required, MinLength(6)]
        public string Contraseña { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Apellido { get; set; } = null!;
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [Required]
        public string TipoUsuario { get; set; } = null!;
    }
}