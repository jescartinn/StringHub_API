using Microsoft.Data.SqlClient;
using Models;

namespace StringHub.Repositories;

public class CuerdaRepository : ICuerdaRepository
{
    private readonly string _connectionString;

    public CuerdaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Cuerda>> GetAllAsync()
    {
        var cuerdas = new List<Cuerda>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT CuerdaId, Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo " +
            "FROM Cuerdas ORDER BY Marca, Modelo", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            cuerdas.Add(MapToCuerda(reader));
        }

        return cuerdas;
    }

    public async Task<IEnumerable<Cuerda>> GetActivasAsync()
    {
        var cuerdas = new List<Cuerda>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT CuerdaId, Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo " +
            "FROM Cuerdas WHERE Activo = 1 ORDER BY Marca, Modelo", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            cuerdas.Add(MapToCuerda(reader));
        }

        return cuerdas;
    }

    public async Task<Cuerda?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT CuerdaId, Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo " +
            "FROM Cuerdas WHERE CuerdaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToCuerda(reader) : null;
    }

    public async Task<IEnumerable<Cuerda>> GetByMarcaAsync(string marca)
    {
        var cuerdas = new List<Cuerda>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT CuerdaId, Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo " +
            "FROM Cuerdas WHERE Marca = @Marca ORDER BY Modelo", connection);
        
        command.Parameters.AddWithValue("@Marca", marca);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            cuerdas.Add(MapToCuerda(reader));
        }

        return cuerdas;
    }

    public async Task<Cuerda> CreateAsync(Cuerda cuerda)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo) " +
            "VALUES (@Marca, @Modelo, @Calibre, @Material, @Color, @Precio, @Stock, @Activo); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, cuerda);

        await connection.OpenAsync();
        cuerda.CuerdaId = (int)await command.ExecuteScalarAsync();
        
        return cuerda;
    }

    public async Task UpdateAsync(Cuerda cuerda)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Cuerdas SET " +
            "Marca = @Marca, Modelo = @Modelo, " +
            "Calibre = @Calibre, Material = @Material, " +
            "Color = @Color, Precio = @Precio, " +
            "Stock = @Stock, Activo = @Activo " +
            "WHERE CuerdaId = @CuerdaId", connection);

        SetParameters(command, cuerda);
        command.Parameters.AddWithValue("@CuerdaId", cuerda.CuerdaId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la cuerda con ID {cuerda.CuerdaId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Cuerdas SET Activo = 0 WHERE CuerdaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
        }
    }

    public async Task UpdateStockAsync(int id, int cantidad)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Cuerdas SET Stock = Stock + @Cantidad WHERE CuerdaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Cantidad", cantidad);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Cuerdas WHERE CuerdaId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private Cuerda MapToCuerda(SqlDataReader reader)
    {
        return new Cuerda
        {
            CuerdaId = reader.GetInt32(reader.GetOrdinal("CuerdaId")),
            Marca = reader.GetString(reader.GetOrdinal("Marca")),
            Modelo = reader.GetString(reader.GetOrdinal("Modelo")),
            Calibre = reader.GetString(reader.GetOrdinal("Calibre")),
            Material = reader.GetString(reader.GetOrdinal("Material")),
            Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
        };
    }

    private void SetParameters(SqlCommand command, Cuerda cuerda)
    {
        command.Parameters.AddWithValue("@Marca", cuerda.Marca);
        command.Parameters.AddWithValue("@Modelo", cuerda.Modelo);
        command.Parameters.AddWithValue("@Calibre", cuerda.Calibre);
        command.Parameters.AddWithValue("@Material", cuerda.Material);
        command.Parameters.AddWithValue("@Color", (object?)cuerda.Color ?? DBNull.Value);
        command.Parameters.AddWithValue("@Precio", cuerda.Precio);
        command.Parameters.AddWithValue("@Stock", cuerda.Stock);
        command.Parameters.AddWithValue("@Activo", cuerda.Activo);
    }
}