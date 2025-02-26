using Microsoft.AspNetCore.Mvc;
using StringHub.DTOs;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenEncordadoController : ControllerBase
    {
        private readonly IOrdenEncordadoService _ordenService;

        public OrdenEncordadoController(IOrdenEncordadoService ordenService)
        {
            _ordenService = ordenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenEncordadoDto>>> GetOrdenes()
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync();
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenEncordadoDto>> GetOrden(int id)
        {
            var orden = await _ordenService.GetOrdenByIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            return Ok(orden);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordadoDto>>> GetOrdenesByUsuario(int usuarioId)
        {
            var ordenes = await _ordenService.GetOrdenesByUsuarioAsync(usuarioId);
            return Ok(ordenes);
        }

        [HttpGet("encordador/{encordadorId}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordadoDto>>> GetOrdenesByEncordador(int encordadorId)
        {
            var ordenes = await _ordenService.GetOrdenesByEncordadorAsync(encordadorId);
            return Ok(ordenes);
        }

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordadoDto>>> GetOrdenesByEstado(string estado)
        {
            try
            {
                var ordenes = await _ordenService.GetOrdenesByEstadoAsync(estado);
                return Ok(ordenes);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrdenEncordadoDto>> CreateOrden(OrdenEncordadoCreateDto ordenDto)
        {
            try
            {
                var newOrden = await _ordenService.CreateOrdenAsync(ordenDto);
                return CreatedAtAction(nameof(GetOrden), new { id = newOrden.OrdenId }, newOrden);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrden(int id, OrdenEncordadoUpdateDto ordenDto)
        {
            try
            {
                await _ordenService.UpdateOrdenAsync(id, ordenDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(int id)
        {
            try
            {
                await _ordenService.DeleteOrdenAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, OrdenEncordadoEstadoUpdateDto estadoDto)
        {
            try
            {
                await _ordenService.UpdateEstadoOrdenAsync(id, estadoDto.Estado, estadoDto.EncordadorId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}