namespace Models;

public class Servicio
{
    public int ServicioId { get; set; }
    public string NombreServicio { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int TiempoEstimado { get; set; }
    public bool Activo { get; set; }

    public Servicio() 
    {
        Activo = true;
    }

    public Servicio(string nombreServicio, decimal precioBase, int tiempoEstimado)
    {
        NombreServicio = nombreServicio;
        PrecioBase = precioBase;
        TiempoEstimado = tiempoEstimado;
        Activo = true;
    }
}