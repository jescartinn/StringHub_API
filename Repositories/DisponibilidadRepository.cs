using Microsoft.Data.SqlClient;
using StringHub.Models;

namespace StringHub.Repositories;

public class DisponibilidadRepository : IDisponibilidadRepository
{
    private readonly string _connectionString;

    public DisponibilidadRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Disponibilidad>> GetAllAsync()
    {
        var disponibilidades = new List<Disponibilidad>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT DisponibilidadId, EncordadorId, DiaSemana, HoraInicio, HoraFin " +
            "FROM Disponibilidad ORDER BY DiaSemana, HoraInicio", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            disponibilidades.Add(MapToDisponibilidad(reader));
        }

        return disponibilidades;
    }

    public async Task<Disponibilidad?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT DisponibilidadId, EncordadorId, DiaSemana, HoraInicio, HoraFin " +
            "FROM Disponibilidad WHERE DisponibilidadId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        return await reader.ReadAsync() ? MapToDisponibilidad(reader) : null;
    }

    public async Task<IEnumerable<Disponibilidad>> GetByEncordadorIdAsync(int encordadorId)
    {
        var disponibilidades = new List<Disponibilidad>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT DisponibilidadId, EncordadorId, DiaSemana, HoraInicio, HoraFin " +
            "FROM Disponibilidad WHERE EncordadorId = @EncordadorId " +
            "ORDER BY DiaSemana, HoraInicio", connection);
        
        command.Parameters.AddWithValue("@EncordadorId", encordadorId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            disponibilidades.Add(MapToDisponibilidad(reader));
        }

        return disponibilidades;
    }

    public async Task<IEnumerable<Disponibilidad>> GetByDiaSemanaAsync(byte diaSemana)
    {
        var disponibilidades = new List<Disponibilidad>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT DisponibilidadId, EncordadorId, DiaSemana, HoraInicio, HoraFin " +
            "FROM Disponibilidad WHERE DiaSemana = @DiaSemana " +
            "ORDER BY HoraInicio", connection);
        
        command.Parameters.AddWithValue("@DiaSemana", diaSemana);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            disponibilidades.Add(MapToDisponibilidad(reader));
        }

        return disponibilidades;
    }

    public async Task<Disponibilidad> CreateAsync(Disponibilidad disponibilidad)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Disponibilidad (EncordadorId, DiaSemana, HoraInicio, HoraFin) " +
            "VALUES (@EncordadorId, @DiaSemana, @HoraInicio, @HoraFin); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", connection);

        SetParameters(command, disponibilidad);

        await connection.OpenAsync();
        disponibilidad.DisponibilidadId = (int)await command.ExecuteScalarAsync();
        
        return disponibilidad;
    }

    public async Task UpdateAsync(Disponibilidad disponibilidad)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Disponibilidad SET " +
            "EncordadorId = @EncordadorId, " +
            "DiaSemana = @DiaSemana, " +
            "HoraInicio = @HoraInicio, " +
            "HoraFin = @HoraFin " +
            "WHERE DisponibilidadId = @DisponibilidadId", connection);

        SetParameters(command, disponibilidad);
        command.Parameters.AddWithValue("@DisponibilidadId", disponibilidad.DisponibilidadId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la disponibilidad con ID {disponibilidad.DisponibilidadId}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "DELETE FROM Disponibilidad WHERE DisponibilidadId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException($"No se encontró la disponibilidad con ID {id}");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Disponibilidad WHERE DisponibilidadId = @Id", connection);
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> HasConflictingScheduleAsync(int encordadorId, byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin, int? disponibilidadId = null)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"
            SELECT COUNT(1) 
            FROM Disponibilidad 
            WHERE EncordadorId = @EncordadorId 
                AND DiaSemana = @DiaSemana 
                AND ((HoraInicio < @HoraFin AND HoraFin > @HoraInicio)
                     OR (HoraInicio = @HoraInicio AND HoraFin = @HoraFin))";

        if (disponibilidadId.HasValue)
        {
            sql += " AND DisponibilidadId != @DisponibilidadId";
        }

        using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@EncordadorId", encordadorId);
        command.Parameters.AddWithValue("@DiaSemana", diaSemana);
        command.Parameters.AddWithValue("@HoraInicio", horaInicio);
        command.Parameters.AddWithValue("@HoraFin", horaFin);
        if (disponibilidadId.HasValue)
        {
            command.Parameters.AddWithValue("@DisponibilidadId", disponibilidadId.Value);
        }

        await connection.OpenAsync();
        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private Disponibilidad MapToDisponibilidad(SqlDataReader reader)
    {
        return new Disponibilidad
        {
            DisponibilidadId = reader.GetInt32(reader.GetOrdinal("DisponibilidadId")),
            EncordadorId = reader.GetInt32(reader.GetOrdinal("EncordadorId")),
            DiaSemana = reader.GetByte(reader.GetOrdinal("DiaSemana")),
            HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("HoraInicio")),
            HoraFin = reader.GetTimeSpan(reader.GetOrdinal("HoraFin"))
        };
    }

    private void SetParameters(SqlCommand command, Disponibilidad disponibilidad)
    {
        command.Parameters.AddWithValue("@EncordadorId", disponibilidad.EncordadorId);
        command.Parameters.AddWithValue("@DiaSemana", disponibilidad.DiaSemana);
        command.Parameters.AddWithValue("@HoraInicio", disponibilidad.HoraInicio);
        command.Parameters.AddWithValue("@HoraFin", disponibilidad.HoraFin);
    }
}