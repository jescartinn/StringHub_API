using Microsoft.AspNetCore.Mvc;
using StringHub.DTOs;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServicios()
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(servicios);
        }

        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServiciosActivos()
        {
            var servicios = await _servicioService.GetServiciosActivosAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetServicio(int id)
        {
            var servicio = await _servicioService.GetServicioByIdAsync(id);

            if (servicio == null)
            {
                return NotFound();
            }

            return Ok(servicio);
        }

        [HttpPost]
        public async Task<ActionResult<ServicioDto>> CreateServicio(ServicioCreateDto servicioDto)
        {
            try
            {
                var newServicio = await _servicioService.CreateServicioAsync(servicioDto);
                return CreatedAtAction(nameof(GetServicio), new { id = newServicio.ServicioId }, newServicio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, ServicioUpdateDto servicioDto)
        {
            try
            {
                await _servicioService.UpdateServicioAsync(id, servicioDto);
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
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                await _servicioService.DeleteServicioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}