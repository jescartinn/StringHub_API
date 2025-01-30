using Models;
namespace StringHub.Repositories;

public interface ICuerdaRepository
{
    Task<IEnumerable<Cuerda>> GetAllAsync();
    Task<IEnumerable<Cuerda>> GetActivasAsync();
    Task<Cuerda?> GetByIdAsync(int id);
    Task<IEnumerable<Cuerda>> GetByMarcaAsync(string marca);
    Task<Cuerda> CreateAsync(Cuerda cuerda);
    Task UpdateAsync(Cuerda cuerda);
    Task DeleteAsync(int id);
    Task UpdateStockAsync(int id, int cantidad);
    Task<bool> ExistsAsync(int id);
}