using Microsoft.AspNetCore.Mvc;
using StringHub.DTOs;
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
        public async Task<ActionResult<IEnumerable<RaquetaDto>>> GetRaquetas()
        {
            var raquetas = await _raquetaService.GetAllRaquetasAsync();
            return Ok(raquetas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RaquetaDto>> GetRaqueta(int id)
        {
            var raqueta = await _raquetaService.GetRaquetaByIdAsync(id);

            if (raqueta == null)
            {
                return NotFound();
            }

            return Ok(raqueta);
        }

        [HttpGet("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<RaquetaDto>>> GetRaquetasByUser(int userId)
        {
            var raquetas = await _raquetaService.GetRaquetasByUserIdAsync(userId);
            return Ok(raquetas);
        }

        [HttpPost]
        public async Task<ActionResult<RaquetaDto>> CreateRaqueta(RaquetaCreateDto raquetaDto)
        {
            try
            {
                var newRaqueta = await _raquetaService.CreateRaquetaAsync(raquetaDto);
                return CreatedAtAction(nameof(GetRaqueta), new { id = newRaqueta.RaquetaId }, newRaqueta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRaqueta(int id, RaquetaUpdateDto raquetaDto)
        {
            try
            {
                await _raquetaService.UpdateRaquetaAsync(id, raquetaDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaqueta(int id)
        {
            try
            {
                await _raquetaService.DeleteRaquetaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}