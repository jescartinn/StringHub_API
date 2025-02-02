using Microsoft.AspNetCore.Mvc;
using Models;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialTensionController : ControllerBase
    {
        private readonly IHistorialTensionService _historialService;

        public HistorialTensionController(IHistorialTensionService historialService)
        {
            _historialService = historialService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialTension>>> GetHistoriales()
        {
            var historiales = await _historialService.GetAllHistorialAsync();
            return Ok(historiales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialTension>> GetHistorial(int id)
        {
            var historial = await _historialService.GetHistorialByIdAsync(id);

            if (historial == null)
            {
                return NotFound();
            }

            return Ok(historial);
        }

        [HttpGet("raqueta/{raquetaId}")]
        public async Task<ActionResult<IEnumerable<HistorialTension>>> GetHistorialByRaqueta(int raquetaId)
        {
            var historiales = await _historialService.GetHistorialByRaquetaAsync(raquetaId);
            return Ok(historiales);
        }

        [HttpGet("orden/{ordenId}")]
        public async Task<ActionResult<IEnumerable<HistorialTension>>> GetHistorialByOrden(int ordenId)
        {
            var historiales = await _historialService.GetHistorialByOrdenAsync(ordenId);
            return Ok(historiales);
        }

        [HttpPost]
        public async Task<ActionResult<HistorialTension>> CreateHistorial(HistorialTension historial)
        {
            try
            {
                var newHistorial = await _historialService.CreateHistorialAsync(historial);
                return CreatedAtAction(nameof(GetHistorial), new { id = newHistorial.HistorialId }, newHistorial);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHistorial(int id, HistorialTension historial)
        {
            if (id != historial.HistorialId)
            {
                return BadRequest();
            }

            try
            {
                await _historialService.UpdateHistorialAsync(id, historial);
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
        public async Task<IActionResult> DeleteHistorial(int id)
        {
            try
            {
                await _historialService.DeleteHistorialAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}