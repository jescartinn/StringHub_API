using Microsoft.Data.SqlClient;
using StringHub.Models;

namespace StringHub.Repositories;

public class OrdenEncordadoRepository : IOrdenEncordadoRepository
{
    private readonly string _connectionString;

    public OrdenEncordadoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<OrdenEncordado>> GetAllAsync()
    {
        var ordenes = new List<OrdenEncordado>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT OrdenId, UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId " +
            "FROM OrdenesEncordado ORDER BY FechaCreacion DESC", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ordenes.Add(MapToOrdenEncordado(reader));
        }

        return ordenes;
    }

    public async Task<OrdenEncordado?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT OrdenId, UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId " +
            "FROM OrdenesEncordado WHERE OrdenId = @Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapToOrdenEncordado(reader) : null;
    }

    public async Task<IEnumerable<OrdenEncordado>> GetByUsuarioIdAsync(int usuarioId)
    {
        var ordenes = new List<OrdenEncordado>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT OrdenId, UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId " +
            "FROM OrdenesEncordado WHERE UsuarioId = @UsuarioId " +
            "ORDER BY FechaCreacion DESC", connection);

        command.Parameters.AddWithValue("@UsuarioId", usuarioId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ordenes.Add(MapToOrdenEncordado(reader));
        }

        return ordenes;
    }

    public async Task<IEnumerable<OrdenEncordado>> GetByEncordadorIdAsync(int encordadorId)
    {
        var ordenes = new List<OrdenEncordado>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT OrdenId, UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId " +
            "FROM OrdenesEncordado WHERE EncordadorId = @EncordadorId " +
            "ORDER BY FechaCreacion DESC", connection);

        command.Parameters.AddWithValue("@EncordadorId", encordadorId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ordenes.Add(MapToOrdenEncordado(reader));
        }

        return ordenes;
    }

    public async Task<IEnumerable<OrdenEncordado>> GetByEstadoAsync(string estado)
    {
        var ordenes = new List<OrdenEncordado>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT OrdenId, UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId " +
            "FROM OrdenesEncordado WHERE Estado = @Estado " +
            "ORDER BY FechaCreacion DESC", connection);

        command.Parameters.AddWithValue("@Estado", estado);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ordenes.Add(MapToOrdenEncordado(reader));
        }

        return ordenes;
    }

    public async Task<OrdenEncordado> CreateAsync(OrdenEncordado orden)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO OrdenesEncordado (UsuarioId, RaquetaId, ServicioId, CuerdaId, " +
            "TensionVertical, TensionHorizontal, Estado, Comentarios, " +
            "FechaCreacion, FechaCompletado, PrecioTotal, EncordadorId) " +
            "VALUES (@UsuarioId, @RaquetaId, @ServicioId, @CuerdaId, " +
            "@TensionVertical, @TensionHorizontal, @Estado, @Comentarios, " +
            "@FechaCreacion, @FechaCompletado, @PrecioTotal, @EncordadorId); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, orden);

        await connection.OpenAsync();
        orden.OrdenId = (int)await command.ExecuteScalarAsync();

        return orden;
    }

    public async Task UpdateAsync(OrdenEncordado orden)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE OrdenesEncordado SET " +
            "UsuarioId = @UsuarioId, RaquetaId = @RaquetaId, " +
            "ServicioId = @ServicioId, CuerdaId = @CuerdaId, " +
            "TensionVertical = @TensionVertical, TensionHorizontal = @TensionHorizontal, " +
            "Estado = @Estado, Comentarios = @Comentarios, " +
            "FechaCompletado = @FechaCompletado, PrecioTotal = @PrecioTotal, " +
            "EncordadorId = @EncordadorId " +
            "WHERE OrdenId = @OrdenId", connection);

        SetParameters(command, orden);
        command.Parameters.AddWithValue("@OrdenId", orden.OrdenId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la orden con ID {orden.OrdenId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE OrdenesEncordado SET Estado = 'Cancelado' WHERE OrdenId = @Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM OrdenesEncordado WHERE OrdenId = @Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task UpdateEstadoAsync(int id, string estado, int? encordadorId = null)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = encordadorId.HasValue
            ? "UPDATE OrdenesEncordado SET Estado = @Estado, EncordadorId = @EncordadorId, " +
              "FechaCompletado = CASE WHEN @Estado = 'Completado' THEN GETDATE() ELSE FechaCompletado END " +
              "WHERE OrdenId = @Id"
            : "UPDATE OrdenesEncordado SET Estado = @Estado, " +
              "FechaCompletado = CASE WHEN @Estado = 'Completado' THEN GETDATE() ELSE FechaCompletado END " +
              "WHERE OrdenId = @Id";

        using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Estado", estado);
        if (encordadorId.HasValue)
        {
            command.Parameters.AddWithValue("@EncordadorId", encordadorId.Value);
        }

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
        }
    }

    private OrdenEncordado MapToOrdenEncordado(SqlDataReader reader)
    {
        return new OrdenEncordado
        {
            OrdenId = reader.GetInt32(reader.GetOrdinal("OrdenId")),
            UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
            RaquetaId = reader.GetInt32(reader.GetOrdinal("RaquetaId")),
            ServicioId = reader.GetInt32(reader.GetOrdinal("ServicioId")),
            CuerdaId = reader.IsDBNull(reader.GetOrdinal("CuerdaId")) ? null : reader.GetInt32(reader.GetOrdinal("CuerdaId")),
            TensionVertical = reader.GetDecimal(reader.GetOrdinal("TensionVertical")),
            TensionHorizontal = reader.IsDBNull(reader.GetOrdinal("TensionHorizontal")) ? null : reader.GetDecimal(reader.GetOrdinal("TensionHorizontal")),
            Estado = reader.GetString(reader.GetOrdinal("Estado")),
            Comentarios = reader.IsDBNull(reader.GetOrdinal("Comentarios")) ? null : reader.GetString(reader.GetOrdinal("Comentarios")),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
            FechaCompletado = reader.IsDBNull(reader.GetOrdinal("FechaCompletado")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaCompletado")),
            PrecioTotal = reader.GetDecimal(reader.GetOrdinal("PrecioTotal")),
            EncordadorId = reader.IsDBNull(reader.GetOrdinal("EncordadorId")) ? null : reader.GetInt32(reader.GetOrdinal("EncordadorId"))
        };
    }

    private void SetParameters(SqlCommand command, OrdenEncordado orden)
    {
        command.Parameters.AddWithValue("@UsuarioId", orden.UsuarioId);
        command.Parameters.AddWithValue("@RaquetaId", orden.RaquetaId);
        command.Parameters.AddWithValue("@ServicioId", orden.ServicioId);
        command.Parameters.AddWithValue("@CuerdaId", (object?)orden.CuerdaId ?? DBNull.Value);
        command.Parameters.AddWithValue("@TensionVertical", orden.TensionVertical);
        command.Parameters.AddWithValue("@TensionHorizontal", (object?)orden.TensionHorizontal ?? DBNull.Value);
        command.Parameters.AddWithValue("@Estado", orden.Estado);
        command.Parameters.AddWithValue("@Comentarios", (object?)orden.Comentarios ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaCreacion", orden.FechaCreacion);
        command.Parameters.AddWithValue("@FechaCompletado", (object?)orden.FechaCompletado ?? DBNull.Value);
        command.Parameters.AddWithValue("@PrecioTotal", orden.PrecioTotal);
        command.Parameters.AddWithValue("@EncordadorId", (object?)orden.EncordadorId ?? DBNull.Value);
    }
}