using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Services;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _service;
        private readonly ILogger<StaffController> _logger;

        public StaffController(IStaffService service, ILogger<StaffController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> GetAll([FromQuery] string registrationNumber)
        {
            _logger.LogInformation("Fetching all staff for registrationNumber: {RegistrationNumber}", registrationNumber);
            var staffList = await _service.GetAll(registrationNumber);
            _logger.LogInformation("Fetched {Count} staff members", staffList.Count);
            return Ok(staffList);
        }

        [HttpGet("{staffId:Guid}")]
        public async Task<ActionResult<StaffDto>> GetById(Guid staffId)
        {
            _logger.LogInformation("Fetching staff by Id: {StaffId}", staffId);
            var item = await _service.GetById(staffId);
            if (item == null)
            {
                _logger.LogWarning("Staff not found: {StaffId}", staffId);
                return NotFound();
            }
            _logger.LogInformation("Fetched staff: {StaffId}", staffId);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create([FromBody] StaffCreateDto dto)
        {
            _logger.LogInformation("Creating new staff with Email: {Email}", dto.Email);
            var created = await _service.Create(dto);
            _logger.LogInformation("Created staff: {StaffId}", created.StaffId);
            return CreatedAtAction(nameof(GetById), new { staffId = created.StaffId }, created);
        }

        [HttpPost("login")]
        public async Task<ActionResult<StaffDto>> Login([FromBody] LoginRequestDto dto)
        {
            _logger.LogInformation("Authenticating staff with Email: {Email}", dto.Email);
            var auth = await _service.AuthenticateAsync(dto.Email, dto.Password);
            if (auth == null)
            {
                _logger.LogWarning("Authentication failed for Email: {Email}", dto.Email);
                return Unauthorized();
            }
            _logger.LogInformation("Authentication successful for StaffId: {StaffId}", auth.StaffId);
            return Ok(auth);
        }

        [HttpPut("{staffId:Guid}")]
        public async Task<ActionResult<StaffDto>> Update(Guid staffId, [FromBody] StaffUpdateDto dto)
        {
            _logger.LogInformation("Updating staff: {StaffId}", staffId);
            var updated = await _service.Update(staffId, dto);
            if (updated == null)
            {
                _logger.LogWarning("Failed to update staff (not found): {StaffId}", staffId);
                return NotFound();
            }
            _logger.LogInformation("Updated staff successfully: {StaffId}", staffId);
            return Ok(updated);
        }

        [HttpDelete("{staffId:Guid}")]
        public async Task<IActionResult> Delete(Guid staffId)
        {
            _logger.LogInformation("Deleting staff: {StaffId}", staffId);
            var success = await _service.Delete(staffId);
            if (!success)
            {
                _logger.LogWarning("Failed to delete staff (not found): {StaffId}", staffId);
                return NotFound();
            }
            _logger.LogInformation("Deleted staff successfully: {StaffId}", staffId);
            return NoContent();
        }
    }
}
