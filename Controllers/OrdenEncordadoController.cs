using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<ActionResult<IEnumerable<OrdenEncordado>>> GetOrdenes()
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync();
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenEncordado>> GetOrden(int id)
        {
            var orden = await _ordenService.GetOrdenByIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            return Ok(orden);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordado>>> GetOrdenesByUsuario(int usuarioId)
        {
            var ordenes = await _ordenService.GetOrdenesByUsuarioAsync(usuarioId);
            return Ok(ordenes);
        }

        [HttpGet("encordador/{encordadorId}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordado>>> GetOrdenesByEncordador(int encordadorId)
        {
            var ordenes = await _ordenService.GetOrdenesByEncordadorAsync(encordadorId);
            return Ok(ordenes);
        }

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<OrdenEncordado>>> GetOrdenesByEstado(string estado)
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
        public async Task<ActionResult<OrdenEncordado>> CreateOrden(OrdenEncordado orden)
        {
            try
            {
                var newOrden = await _ordenService.CreateOrdenAsync(orden);
                return CreatedAtAction(nameof(GetOrden), new { id = newOrden.OrdenId }, newOrden);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrden(int id, OrdenEncordado orden)
        {
            if (id != orden.OrdenId)
            {
                return BadRequest();
            }

            try
            {
                await _ordenService.UpdateOrdenAsync(id, orden);
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

        [HttpPut("{id}/estado/{estado}")]
        public async Task<IActionResult> UpdateEstado(int id, string estado)
        {
            try
            {
                await _ordenService.UpdateEstadoOrdenAsync(id, estado);
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