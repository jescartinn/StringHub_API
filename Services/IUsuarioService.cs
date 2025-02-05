using StringHub.Models;
namespace StringHub.Services;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<Usuario> CreateUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(int id, Usuario usuario);
    Task DeleteUsuarioAsync(int id);
    Task<bool> ValidateUsuarioExistsAsync(int id);
    Task<bool> ValidateEmailExistsAsync(string email);
}