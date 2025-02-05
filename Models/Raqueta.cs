namespace StringHub.Models;
public class Raqueta
{
    public int RaquetaId { get; set; }
    public int UsuarioId { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string? NumeroSerie { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Raqueta() {} //parameterless constructor

    public Raqueta(string marca, string modelo)
    {
        Marca = marca;
        Modelo = modelo;
        FechaCreacion = DateTime.UtcNow;
    }
}