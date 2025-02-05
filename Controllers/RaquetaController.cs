using Microsoft.AspNetCore.Mvc;
using StringHub.Models;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaquetaController : ControllerBase
    {
        private readonly IRaquetaService _raquetaService;

        public RaquetaController(IRaquetaService raquetaService)
        {
            _raquetaService = raquetaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Raqueta>>> GetRaquetas()
        {
            var raquetas = await _raquetaService.GetAllRaquetasAsync();
            return Ok(raquetas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Raqueta>> GetRaqueta(int id)
        {
            var raqueta = await _raquetaService.GetRaquetaByIdAsync(id);

            if (raqueta == null)
            {
                return NotFound();
            }

            return Ok(raqueta);
        }

        [HttpGet("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Raqueta>>> GetRaquetasByUser(int userId)
        {
            var raquetas = await _raquetaService.GetRaquetasByUserIdAsync(userId);
            return Ok(raquetas);
        }

        [HttpPost]
        public async Task<ActionResult<Raqueta>> CreateRaqueta(Raqueta raqueta)
        {
            var newRaqueta = await _raquetaService.CreateRaquetaAsync(raqueta);
            return CreatedAtAction(nameof(GetRaqueta), new { id = newRaqueta.RaquetaId }, newRaqueta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRaqueta(int id, Raqueta raqueta)
        {
            if (id != raqueta.RaquetaId)
            {
                return BadRequest();
            }

            var exists = await _raquetaService.ValidateRaquetaExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _raquetaService.UpdateRaquetaAsync(id, raqueta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaqueta(int id)
        {
            var exists = await _raquetaService.ValidateRaquetaExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _raquetaService.DeleteRaquetaAsync(id);
            return NoContent();
        }
    }
}