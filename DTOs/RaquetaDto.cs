namespace StringHub.DTOs
{
    public class RaquetaDto
    {
        public int RaquetaId { get; set; }
        public int UsuarioId { get; set; }
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string? NumeroSerie { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}