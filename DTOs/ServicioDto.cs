namespace StringHub.DTOs
{
    public class ServicioDto
    {
        public int ServicioId { get; set; }
        public string NombreServicio { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int TiempoEstimado { get; set; }
        public bool Activo { get; set; }
    }
}