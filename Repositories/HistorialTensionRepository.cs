using Microsoft.Data.SqlClient;
using Models;

namespace StringHub.Repositories;

public class HistorialTensionRepository : IHistorialTensionRepository
{
    private readonly string _connectionString;

    public HistorialTensionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<HistorialTension>> GetAllAsync()
    {
        var historiales = new List<HistorialTension>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT HistorialId, RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha " +
            "FROM HistorialTensiones ORDER BY Fecha DESC", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            historiales.Add(MapToHistorialTension(reader));
        }

        return historiales;
    }

    public async Task<HistorialTension?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT HistorialId, RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha " +
            "FROM HistorialTensiones WHERE HistorialId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToHistorialTension(reader) : null;
    }

    public async Task<IEnumerable<HistorialTension>> GetByRaquetaIdAsync(int raquetaId)
    {
        var historiales = new List<HistorialTension>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT HistorialId, RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha " +
            "FROM HistorialTensiones WHERE RaquetaId = @RaquetaId " +
            "ORDER BY Fecha DESC", connection);
        
        command.Parameters.AddWithValue("@RaquetaId", raquetaId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            historiales.Add(MapToHistorialTension(reader));
        }

        return historiales;
    }

    public async Task<IEnumerable<HistorialTension>> GetByOrdenIdAsync(int ordenId)
    {
        var historiales = new List<HistorialTension>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT HistorialId, RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha " +
            "FROM HistorialTensiones WHERE OrdenId = @OrdenId " +
            "ORDER BY Fecha DESC", connection);
        
        command.Parameters.AddWithValue("@OrdenId", ordenId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            historiales.Add(MapToHistorialTension(reader));
        }

        return historiales;
    }

    public async Task<HistorialTension> CreateAsync(HistorialTension historial)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO HistorialTensiones (RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha) " +
            "VALUES (@RaquetaId, @OrdenId, @TensionVertical, @TensionHorizontal, @CuerdaId, @Fecha); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, historial);

        await connection.OpenAsync();
        historial.HistorialId = (int)await command.ExecuteScalarAsync();
        
        return historial;
    }

    public async Task UpdateAsync(HistorialTension historial)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE HistorialTensiones SET " +
            "RaquetaId = @RaquetaId, " +
            "OrdenId = @OrdenId, " +
            "TensionVertical = @TensionVertical, " +
            "TensionHorizontal = @TensionHorizontal, " +
            "CuerdaId = @CuerdaId, " +
            "Fecha = @Fecha " +
            "WHERE HistorialId = @HistorialId", connection);

        SetParameters(command, historial);
        command.Parameters.AddWithValue("@HistorialId", historial.HistorialId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el historial con ID {historial.HistorialId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "DELETE FROM HistorialTensiones WHERE HistorialId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró el historial con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM HistorialTensiones WHERE HistorialId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private HistorialTension MapToHistorialTension(SqlDataReader reader)
    {
        return new HistorialTension
        {
            HistorialId = reader.GetInt32(reader.GetOrdinal("HistorialId")),
            RaquetaId = reader.GetInt32(reader.GetOrdinal("RaquetaId")),
            OrdenId = reader.GetInt32(reader.GetOrdinal("OrdenId")),
            TensionVertical = reader.GetDecimal(reader.GetOrdinal("TensionVertical")),
            TensionHorizontal = reader.IsDBNull(reader.GetOrdinal("TensionHorizontal")) ? null : reader.GetDecimal(reader.GetOrdinal("TensionHorizontal")),
            CuerdaId = reader.IsDBNull(reader.GetOrdinal("CuerdaId")) ? null : reader.GetInt32(reader.GetOrdinal("CuerdaId")),
            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"))
        };
    }

    private void SetParameters(SqlCommand command, HistorialTension historial)
    {
        command.Parameters.AddWithValue("@RaquetaId", historial.RaquetaId);
        command.Parameters.AddWithValue("@OrdenId", historial.OrdenId);
        command.Parameters.AddWithValue("@TensionVertical", historial.TensionVertical);
        command.Parameters.AddWithValue("@TensionHorizontal", (object?)historial.TensionHorizontal ?? DBNull.Value);
        command.Parameters.AddWithValue("@CuerdaId", (object?)historial.CuerdaId ?? DBNull.Value);
        command.Parameters.AddWithValue("@Fecha", historial.Fecha);
    }
}