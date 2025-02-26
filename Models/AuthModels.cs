using System.ComponentModel.DataAnnotations;

namespace StringHub.Models
{
    public class AuthRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
    }

    public class RegisterRequest
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Apellido { get; set; } = null!;
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [Required]
        public string TipoUsuario { get; set; } = "Cliente"; // Por defecto todos los registros son de tipo Cliente
    }
}