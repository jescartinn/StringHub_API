using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.Models;
using System.Security.Cryptography;
using System.Text;

namespace StringHub.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
                
            if (usuario == null || !VerifyPasswordHash(request.Password, usuario.Contraseña))
            {
                return null; // Credenciales inválidas
            }

            return _jwtService.GenerateToken(usuario);
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            // Verificar si el email ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                return null; // Email ya registrado
            }

            // Crear usuario
            var nuevoUsuario = new Usuario
            {
                Email = request.Email,
                Contraseña = HashPassword(request.Password),
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                TipoUsuario = request.TipoUsuario,
                FechaCreacion = DateTime.UtcNow,
                UltimaModificacion = DateTime.UtcNow
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            // Generar token
            return _jwtService.GenerateToken(nuevoUsuario);
        }

        private string HashPassword(string password)
        {
            // Simple hashing con SHA256, pero en producción deberías usar algo como BCrypt
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Verificar que el hash coincida
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }
}