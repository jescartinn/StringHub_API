using Microsoft.Data.SqlClient;
using Models;

namespace StringHub.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        var usuarios = new List<Usuario>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT UsuarioId, Email, Contraseña, Nombre, Apellido, Telefono, TipoUsuario, FechaCreacion, UltimaModificacion " +
            "FROM Usuarios ORDER BY UsuarioId", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            usuarios.Add(MapToUsuario(reader));
        }

        return usuarios;
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT UsuarioId, Email, Contraseña, Nombre, Apellido, Telefono, TipoUsuario, FechaCreacion, UltimaModificacion " +
            "FROM Usuarios WHERE UsuarioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToUsuario(reader) : null;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT UsuarioId, Email, Contraseña, Nombre, Apellido, Telefono, TipoUsuario, FechaCreacion, UltimaModificacion " +
            "FROM Usuarios WHERE Email = @Email", connection);
        
        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToUsuario(reader) : null;
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Usuarios (Email, Contraseña, Nombre, Apellido, Telefono, TipoUsuario, FechaCreacion, UltimaModificacion) " +
            "VALUES (@Email, @Contraseña, @Nombre, @Apellido, @Telefono, @TipoUsuario, @FechaCreacion, @UltimaModificacion); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, usuario);

        await connection.OpenAsync();
        usuario.UsuarioId = (int)await command.ExecuteScalarAsync();
        
        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Usuarios SET " +
            "Email = @Email, Contraseña = @Contraseña, " +
            "Nombre = @Nombre, Apellido = @Apellido, " +
            "Telefono = @Telefono, TipoUsuario = @TipoUsuario, " +
            "UltimaModificacion = @UltimaModificacion " +
            "WHERE UsuarioId = @UsuarioId", connection);

        SetParameters(command, usuario);
        command.Parameters.AddWithValue("@UsuarioId", usuario.UsuarioId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el usuario con ID {usuario.UsuarioId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "DELETE FROM Usuarios WHERE UsuarioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Usuarios WHERE UsuarioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Usuarios WHERE Email = @Email", connection);
        
        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private Usuario MapToUsuario(SqlDataReader reader)
    {
        return new Usuario
        {
            UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
            TipoUsuario = reader.GetString(reader.GetOrdinal("TipoUsuario")),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
            UltimaModificacion = reader.GetDateTime(reader.GetOrdinal("UltimaModificacion"))
        };
    }

    private void SetParameters(SqlCommand command, Usuario usuario)
    {
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@Telefono", (object?)usuario.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@TipoUsuario", usuario.TipoUsuario);
        command.Parameters.AddWithValue("@FechaCreacion", usuario.FechaCreacion);
        command.Parameters.AddWithValue("@UltimaModificacion", usuario.UltimaModificacion);
    }
}