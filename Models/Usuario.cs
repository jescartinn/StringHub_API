namespace Models;
public class Usuario
{
    public int UsuarioId { get; set; }
    public string Email { get; set; }
    public string Contraseña { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string? Telefono { get; set; }
    public string TipoUsuario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimaModificacion { get; set; }

    public Usuario() {} //parameterless constructor

    public Usuario(string email, string nombre, string apellido, string tipoUsuario)
    {
        Email = email;
        Nombre = nombre;
        Apellido = apellido;
        TipoUsuario = tipoUsuario;
        FechaCreacion = DateTime.UtcNow;
        UltimaModificacion = DateTime.UtcNow;
    }
}