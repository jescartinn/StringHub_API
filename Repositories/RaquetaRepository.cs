using Microsoft.Data.SqlClient;
using Models;

namespace StringHub.Repositories;

public class RaquetaRepository : IRaquetaRepository
{
    private readonly string _connectionString;

    public RaquetaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Raqueta>> GetAllAsync()
    {
        var raquetas = new List<Raqueta>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT RaquetaId, UsuarioId, Marca, Modelo, NumeroSerie, Descripcion, FechaCreacion " +
            "FROM Raquetas ORDER BY Marca", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            raquetas.Add(MapToRaqueta(reader));
        }

        return raquetas;
    }

    public async Task<Raqueta?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT RaquetaId, UsuarioId, Marca, Modelo, NumeroSerie, Descripcion, FechaCreacion " +
            "FROM Raquetas WHERE RaquetaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToRaqueta(reader) : null;
    }

    public async Task<IEnumerable<Raqueta>> GetByUserIdAsync(int userId)
    {
        var raquetas = new List<Raqueta>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT RaquetaId, UsuarioId, Marca, Modelo, NumeroSerie, Descripcion, FechaCreacion " +
            "FROM Raquetas WHERE UsuarioId = @UsuarioId ORDER BY FechaCreacion DESC", connection);
        
        command.Parameters.AddWithValue("@UsuarioId", userId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            raquetas.Add(MapToRaqueta(reader));
        }

        return raquetas;
    }

    public async Task<Raqueta> CreateAsync(Raqueta raqueta)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Raquetas (UsuarioId, Marca, Modelo, NumeroSerie, Descripcion, FechaCreacion) " +
            "VALUES (@UsuarioId, @Marca, @Modelo, @NumeroSerie, @Descripcion, @FechaCreacion); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, raqueta);

        await connection.OpenAsync();
        raqueta.RaquetaId = (int)await command.ExecuteScalarAsync();
        
        return raqueta;
    }

    public async Task UpdateAsync(Raqueta raqueta)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Raquetas SET " +
            "UsuarioId = @UsuarioId, Marca = @Marca, Modelo = @Modelo, " +
            "NumeroSerie = @NumeroSerie, Descripcion = @Descripcion, " +
            "FechaCreacion = @FechaCreacion " +
            "WHERE RaquetaId = @RaquetaId", connection);

        SetParameters(command, raqueta);
        command.Parameters.AddWithValue("@RaquetaId", raqueta.RaquetaId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la raqueta con ID {raqueta.RaquetaId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "DELETE FROM Raquetas WHERE RaquetaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la raqueta con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Raquetas WHERE RaquetaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private Raqueta MapToRaqueta(SqlDataReader reader)
    {
        return new Raqueta
        {
            RaquetaId = reader.GetInt32(reader.GetOrdinal("RaquetaId")),
            UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
            Marca = reader.GetString(reader.GetOrdinal("Marca")),
            Modelo = reader.GetString(reader.GetOrdinal("Modelo")),
            NumeroSerie = reader.IsDBNull(reader.GetOrdinal("NumeroSerie")) ? null : reader.GetString(reader.GetOrdinal("NumeroSerie")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
        };
    }

    private void SetParameters(SqlCommand command, Raqueta raqueta)
    {
        command.Parameters.AddWithValue("@Marca", raqueta.Marca);
        command.Parameters.AddWithValue("@Modelo", raqueta.Modelo);
        command.Parameters.AddWithValue("@NumeroSerie", (object?)raqueta.NumeroSerie ?? DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", (object?)raqueta.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@UsuarioId", raqueta.UsuarioId);
        command.Parameters.AddWithValue("@FechaCreacion", raqueta.FechaCreacion);
    }
}