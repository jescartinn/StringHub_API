using Microsoft.AspNetCore.Mvc;
using StringHub.Models;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuerdaController : ControllerBase
    {
        private readonly ICuerdaService _cuerdaService;

        public CuerdaController(ICuerdaService cuerdaService)
        {
            _cuerdaService = cuerdaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuerda>>> GetCuerdas()
        {
            var cuerdas = await _cuerdaService.GetAllCuerdasAsync();
            return Ok(cuerdas);
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<Cuerda>>> GetCuerdasActivas()
        {
            var cuerdas = await _cuerdaService.GetCuerdasActivasAsync();
            return Ok(cuerdas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cuerda>> GetCuerda(int id)
        {
            var cuerda = await _cuerdaService.GetCuerdaByIdAsync(id);

            if (cuerda == null)
            {
                return NotFound();
            }

            return Ok(cuerda);
        }

        [HttpGet("marca/{marca}")]
        public async Task<ActionResult<IEnumerable<Cuerda>>> GetCuerdasByMarca(string marca)
        {
            var cuerdas = await _cuerdaService.GetCuerdasByMarcaAsync(marca);
            return Ok(cuerdas);
        }

        [HttpPost]
        public async Task<ActionResult<Cuerda>> CreateCuerda(Cuerda cuerda)
        {
            try
            {
                var newCuerda = await _cuerdaService.CreateCuerdaAsync(cuerda);
                return CreatedAtAction(nameof(GetCuerda), new { id = newCuerda.CuerdaId }, newCuerda);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCuerda(int id, Cuerda cuerda)
        {
            if (id != cuerda.CuerdaId)
            {
                return BadRequest();
            }

            try
            {
                await _cuerdaService.UpdateCuerdaAsync(id, cuerda);
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
        public async Task<IActionResult> DeleteCuerda(int id)
        {
            try
            {
                await _cuerdaService.DeleteCuerdaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int cantidad)
        {
            try
            {
                await _cuerdaService.UpdateStockCuerdaAsync(id, cantidad);
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