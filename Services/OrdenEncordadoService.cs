using StringHub.Repositories;
using StringHub.Models;

namespace StringHub.Services
{
    public class OrdenEncordadoService : IOrdenEncordadoService
    {
        private readonly IOrdenEncordadoRepository _ordenRepository;

        public OrdenEncordadoService(IOrdenEncordadoRepository ordenRepository)
        {
            _ordenRepository = ordenRepository;
        }

        public async Task<IEnumerable<OrdenEncordado>> GetAllOrdenesAsync()
        {
            return await _ordenRepository.GetAllAsync();
        }

        public async Task<OrdenEncordado?> GetOrdenByIdAsync(int id)
        {
            return await _ordenRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OrdenEncordado>> GetOrdenesByUsuarioAsync(int usuarioId)
        {
            return await _ordenRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<IEnumerable<OrdenEncordado>> GetOrdenesByEncordadorAsync(int encordadorId)
        {
            return await _ordenRepository.GetByEncordadorIdAsync(encordadorId);
        }

        public async Task<IEnumerable<OrdenEncordado>> GetOrdenesByEstadoAsync(string estado)
        {
            if (!IsValidEstado(estado))
            {
                throw new InvalidOperationException($"Estado inválido: {estado}");
            }
            return await _ordenRepository.GetByEstadoAsync(estado);
        }

        public async Task<OrdenEncordado> CreateOrdenAsync(OrdenEncordado orden)
        {
            ValidateOrden(orden);

            if (string.IsNullOrEmpty(orden.Estado))
            {
                orden.Estado = "Pendiente";
            }

            orden.FechaCreacion = DateTime.UtcNow;

            return await _ordenRepository.CreateAsync(orden);
        }

        public async Task UpdateOrdenAsync(int id, OrdenEncordado orden)
        {
            if (id != orden.OrdenId)
            {
                throw new ArgumentException("El ID de la orden no coincide con el ID proporcionado");
            }

            ValidateOrden(orden);
            await _ordenRepository.UpdateAsync(orden);
        }

        public async Task DeleteOrdenAsync(int id)
        {
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
            }

            if (orden.Estado != "Pendiente")
            {
                throw new InvalidOperationException("Solo se pueden eliminar órdenes pendientes");
            }

            await _ordenRepository.DeleteAsync(id);
        }

        public async Task UpdateEstadoOrdenAsync(int id, string estado, int? encordadorId = null)
        {
            if (!IsValidEstado(estado))
            {
                throw new InvalidOperationException($"Estado inválido: {estado}");
            }

            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
            }

            if (!IsValidEstadoTransition(orden.Estado, estado))
            {
                throw new InvalidOperationException($"Transición de estado inválida: {orden.Estado} -> {estado}");
            }

            if (estado == "En Proceso" && !encordadorId.HasValue)
            {
                throw new InvalidOperationException("Se requiere un encordador para cambiar el estado a 'En Proceso'");
            }

            await _ordenRepository.UpdateEstadoAsync(id, estado, encordadorId);
        }

        public async Task<bool> ValidateOrdenExistsAsync(int id)
        {
            return await _ordenRepository.ExistsAsync(id);
        }

        private void ValidateOrden(OrdenEncordado orden)
        {
            var errors = new List<string>();

            if (orden.TensionVertical <= 0)
                errors.Add("La tensión vertical debe ser mayor que 0");

            if (orden.TensionHorizontal.HasValue && orden.TensionHorizontal <= 0)
                errors.Add("La tensión horizontal debe ser mayor que 0");

            if (orden.PrecioTotal <= 0)
                errors.Add("El precio total debe ser mayor que 0");

            if (!string.IsNullOrEmpty(orden.Estado) && !IsValidEstado(orden.Estado))
                errors.Add($"Estado inválido: {orden.Estado}");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }

        private bool IsValidEstado(string estado)
        {
            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Entregado", "Cancelado" };
            return estadosValidos.Contains(estado);
        }

        private bool IsValidEstadoTransition(string estadoActual, string nuevoEstado)
        {
            var transicionesValidas = new Dictionary<string, string[]>
            {
                { "Pendiente", new[] { "En Proceso", "Cancelado" } },
                { "En Proceso", new[] { "Completado", "Cancelado" } },
                { "Completado", new[] { "Entregado" } },
                { "Entregado", new string[] { } },
                { "Cancelado", new string[] { } }
            };

            return transicionesValidas.ContainsKey(estadoActual) &&
                   transicionesValidas[estadoActual].Contains(nuevoEstado);
        }
    }
}