namespace StringHub.DTOs
{
    public class CuerdaDto
    {
        public int CuerdaId { get; set; }
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string Calibre { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string? Color { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
    }
}