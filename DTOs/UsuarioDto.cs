namespace StringHub.DTOs
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Telefono { get; set; }
        public string TipoUsuario { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}