using Microsoft.AspNetCore.Mvc;
using StringHub.DTOs;
using StringHub.Services;

namespace StringHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IDisponibilidadService _disponibilidadService;

        public DisponibilidadController(IDisponibilidadService disponibilidadService)
        {
            _disponibilidadService = disponibilidadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisponibilidadDto>>> GetDisponibilidades()
        {
            var disponibilidades = await _disponibilidadService.GetAllDisponibilidadesAsync();
            return Ok(disponibilidades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisponibilidadDto>> GetDisponibilidad(int id)
        {
            var disponibilidad = await _disponibilidadService.GetDisponibilidadByIdAsync(id);

            if (disponibilidad == null)
            {
                return NotFound();
            }

            return Ok(disponibilidad);
        }

        [HttpGet("encordador/{encordadorId}")]
        public async Task<ActionResult<IEnumerable<DisponibilidadDto>>> GetDisponibilidadesByEncordador(int encordadorId)
        {
            var disponibilidades = await _disponibilidadService.GetDisponibilidadesByEncordadorAsync(encordadorId);
            return Ok(disponibilidades);
        }

        [HttpGet("dia/{diaSemana}")]
        public async Task<ActionResult<IEnumerable<DisponibilidadDto>>> GetDisponibilidadesByDiaSemana(byte diaSemana)
        {
            try
            {
                var disponibilidades = await _disponibilidadService.GetDisponibilidadesByDiaSemanaAsync(diaSemana);
                return Ok(disponibilidades);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DisponibilidadDto>> CreateDisponibilidad(DisponibilidadCreateDto disponibilidadDto)
        {
            try
            {
                var newDisponibilidad = await _disponibilidadService.CreateDisponibilidadAsync(disponibilidadDto);
                return CreatedAtAction(nameof(GetDisponibilidad), new { id = newDisponibilidad.DisponibilidadId }, newDisponibilidad);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisponibilidad(int id, DisponibilidadUpdateDto disponibilidadDto)
        {
            try
            {
                await _disponibilidadService.UpdateDisponibilidadAsync(id, disponibilidadDto);
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
        public async Task<IActionResult> DeleteDisponibilidad(int id)
        {
            try
            {
                await _disponibilidadService.DeleteDisponibilidadAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}