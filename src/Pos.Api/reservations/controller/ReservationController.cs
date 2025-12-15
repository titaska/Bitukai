using Microsoft.AspNetCore.Mvc;
using Pos.Api.reservations.dto;
using Pos.Api.reservations.service;

namespace Pos.Api.reservations.controller
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;

     

        public ReservationController(ReservationService service)
        {
            _service = service;
        }




        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability(
        [FromQuery] int employeeId,
        [FromQuery] DateTime date)
        {
            var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var takenSlots = await _service.GetTakenSlotsAsync(employeeId, utcDate);
            return Ok(takenSlots);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var r = await _service.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, created);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromQuery] string status)
        {
            await _service.UpdateStatusAsync(id, status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
