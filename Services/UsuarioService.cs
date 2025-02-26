using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;
            
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> GetUsuarioByEmailAsync(string email)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                
            if (usuario == null) return null;
            
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuarioDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == usuarioDto.Email.ToLower()))
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {usuarioDto.Email}");
            }

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.FechaCreacion = DateTime.UtcNow;
            usuario.UltimaModificacion = DateTime.UtcNow;
            
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task UpdateUsuarioAsync(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");
            }

            if (!string.IsNullOrEmpty(usuarioDto.Email) && 
                usuario.Email.ToLower() != usuarioDto.Email.ToLower() &&
                await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == usuarioDto.Email.ToLower()))
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {usuarioDto.Email}");
            }

            _mapper.Map(usuarioDto, usuario);
            usuario.UltimaModificacion = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateUsuarioExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.UsuarioId == id);
        }

        public async Task<bool> ValidateEmailExistsAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}