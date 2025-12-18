using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.reservations.dto;
using Pos.Api.reservations.service;

namespace Pos.Api.reservations.controller
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ReservationService service, ILogger<ReservationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability([FromQuery] Guid employeeId, [FromQuery] DateTime date)
        {
            _logger.LogInformation("Fetching availability for employee {EmployeeId} on date {Date}", employeeId, date);

            var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var takenSlots = await _service.GetTakenSlotsAsync(employeeId, utcDate);

            _logger.LogInformation("Found {Count} taken slots for employee {EmployeeId} on date {Date}", takenSlots.Count, employeeId, date);
            return Ok(takenSlots);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all reservations");
            var reservations = await _service.GetAllAsync();
            _logger.LogInformation("Found {Count} reservations", reservations.Count());
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("Fetching reservation with ID {ReservationId}", id);
            var reservation = await _service.GetByIdAsync(id);

            if (reservation == null)
            {
                _logger.LogWarning("Reservation not found: {ReservationId}", id);
                return NotFound();
            }

            _logger.LogInformation("Found reservation {ReservationId}", id);
            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            _logger.LogInformation("Creating reservation for employee {EmployeeId} on {Date}", dto.EmployeeId, dto.StartTime);

            var created = await _service.CreateAsync(dto);

            _logger.LogInformation("Created reservation {ReservationId}", created.AppointmentId);
            return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, created);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromQuery] string status)
        {
            _logger.LogInformation("Updating status of reservation {ReservationId} to {Status}", id, status);

            await _service.UpdateStatusAsync(id, status);

            _logger.LogInformation("Updated status of reservation {ReservationId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting reservation {ReservationId}", id);

            await _service.DeleteAsync(id);

            _logger.LogInformation("Deleted reservation {ReservationId}", id);
            return NoContent();
        }
    }
}
