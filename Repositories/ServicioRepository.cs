using Microsoft.Data.SqlClient;
using StringHub.Models;

namespace StringHub.Repositories;

public class ServicioRepository : IServicioRepository
{
    private readonly string _connectionString;

    public ServicioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Servicio>> GetAllAsync()
    {
        var servicios = new List<Servicio>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT ServicioId, NombreServicio, Descripcion, PrecioBase, TiempoEstimado, Activo " +
            "FROM Servicios ORDER BY ServicioId", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            servicios.Add(MapToServicio(reader));
        }

        return servicios;
    }

    public async Task<IEnumerable<Servicio>> GetActivosAsync()
    {
        var servicios = new List<Servicio>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT ServicioId, NombreServicio, Descripcion, PrecioBase, TiempoEstimado, Activo " +
            "FROM Servicios WHERE Activo = 1 ORDER BY ServicioId", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            servicios.Add(MapToServicio(reader));
        }

        return servicios;
    }

    public async Task<Servicio?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT ServicioId, NombreServicio, Descripcion, PrecioBase, TiempoEstimado, Activo " +
            "FROM Servicios WHERE ServicioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToServicio(reader) : null;
    }

    public async Task<Servicio> CreateAsync(Servicio servicio)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Servicios (NombreServicio, Descripcion, PrecioBase, TiempoEstimado, Activo) " +
            "VALUES (@NombreServicio, @Descripcion, @PrecioBase, @TiempoEstimado, @Activo); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, servicio);

        await connection.OpenAsync();
        servicio.ServicioId = (int)await command.ExecuteScalarAsync();
        
        return servicio;
    }

    public async Task UpdateAsync(Servicio servicio)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Servicios SET " +
            "NombreServicio = @NombreServicio, " +
            "Descripcion = @Descripcion, " +
            "PrecioBase = @PrecioBase, " +
            "TiempoEstimado = @TiempoEstimado, " +
            "Activo = @Activo " +
            "WHERE ServicioId = @ServicioId", connection);

        SetParameters(command, servicio);
        command.Parameters.AddWithValue("@ServicioId", servicio.ServicioId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el servicio con ID {servicio.ServicioId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Servicios SET Activo = 0 WHERE ServicioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el servicio con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Servicios WHERE ServicioId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private Servicio MapToServicio(SqlDataReader reader)
    {
        return new Servicio
        {
            ServicioId = reader.GetInt32(reader.GetOrdinal("ServicioId")),
            NombreServicio = reader.GetString(reader.GetOrdinal("NombreServicio")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
            PrecioBase = reader.GetDecimal(reader.GetOrdinal("PrecioBase")),
            TiempoEstimado = reader.GetInt32(reader.GetOrdinal("TiempoEstimado")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
        };
    }

    private void SetParameters(SqlCommand command, Servicio servicio)
    {
        command.Parameters.AddWithValue("@NombreServicio", servicio.NombreServicio);
        command.Parameters.AddWithValue("@Descripcion", (object?)servicio.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@PrecioBase", servicio.PrecioBase);
        command.Parameters.AddWithValue("@TiempoEstimado", servicio.TiempoEstimado);
        command.Parameters.AddWithValue("@Activo", servicio.Activo);
    }
}