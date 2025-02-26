using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
        Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<UsuarioDto?> GetUsuarioByEmailAsync(string email);
        Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuario);
        Task UpdateUsuarioAsync(int id, UsuarioUpdateDto usuario);
        Task DeleteUsuarioAsync(int id);
        Task<bool> ValidateUsuarioExistsAsync(int id);
        Task<bool> ValidateEmailExistsAsync(string email);
    }
}