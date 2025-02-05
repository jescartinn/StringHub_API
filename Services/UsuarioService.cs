using StringHub.Repositories;
using StringHub.Models;

namespace StringHub.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            if (await _usuarioRepository.EmailExistsAsync(usuario.Email))
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {usuario.Email}");
            }

            usuario.FechaCreacion = DateTime.UtcNow;
            usuario.UltimaModificacion = DateTime.UtcNow;
            return await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task UpdateUsuarioAsync(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                throw new ArgumentException("El ID del usuario no coincide con el ID proporcionado");
            }

            var existingUsuario = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (existingUsuario != null && existingUsuario.UsuarioId != id)
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {usuario.Email}");
            }

            usuario.UltimaModificacion = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateUsuarioExistsAsync(int id)
        {
            return await _usuarioRepository.ExistsAsync(id);
        }

        public async Task<bool> ValidateEmailExistsAsync(string email)
        {
            return await _usuarioRepository.EmailExistsAsync(email);
        }
    }
}