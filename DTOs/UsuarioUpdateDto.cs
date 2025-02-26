using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class UsuarioUpdateDto
    {
        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }
        
        public string? Contraseña { get; set; }
        
        [MaxLength(50)]
        public string? Nombre { get; set; }
        
        [MaxLength(50)]
        public string? Apellido { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        public string? TipoUsuario { get; set; }
    }
}