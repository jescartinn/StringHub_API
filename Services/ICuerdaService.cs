using Models;
namespace StringHub.Services;

public interface ICuerdaService
{
    Task<IEnumerable<Cuerda>> GetAllCuerdasAsync();
    Task<IEnumerable<Cuerda>> GetCuerdasActivasAsync();
    Task<Cuerda?> GetCuerdaByIdAsync(int id);
    Task<IEnumerable<Cuerda>> GetCuerdasByMarcaAsync(string marca);
    Task<Cuerda> CreateCuerdaAsync(Cuerda cuerda);
    Task UpdateCuerdaAsync(int id, Cuerda cuerda);
    Task DeleteCuerdaAsync(int id);
    Task UpdateStockCuerdaAsync(int id, int cantidad);
    Task<bool> ValidateCuerdaExistsAsync(int id);
}