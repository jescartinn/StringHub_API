using StringHub.DTOs;

namespace StringHub.Services
{
    public interface ICuerdaService
    {
        Task<IEnumerable<CuerdaDto>> GetAllCuerdasAsync();
        Task<IEnumerable<CuerdaDto>> GetCuerdasActivasAsync();
        Task<CuerdaDto?> GetCuerdaByIdAsync(int id);
        Task<IEnumerable<CuerdaDto>> GetCuerdasByMarcaAsync(string marca);
        Task<CuerdaDto> CreateCuerdaAsync(CuerdaCreateDto cuerda);
        Task UpdateCuerdaAsync(int id, CuerdaUpdateDto cuerda);
        Task DeleteCuerdaAsync(int id);
        Task UpdateStockCuerdaAsync(int id, int cantidad);
        Task<bool> ValidateCuerdaExistsAsync(int id);
    }
}