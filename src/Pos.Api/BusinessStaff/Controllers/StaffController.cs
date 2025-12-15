using Microsoft.AspNetCore.Mvc;
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
        public StaffController(IStaffService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> GetAll([FromQuery] string registrationNumber)
            => Ok(await _service.GetAll(registrationNumber));

        [HttpGet("{staffId:int}")]
        public async Task<ActionResult<StaffDto>> GetById(int staffId)
        {
            var item = await _service.GetById(staffId);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create([FromBody] StaffCreateDto dto)
        {
            var created = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { staffId = created.StaffId }, created);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<StaffDto>> Login([FromBody] LoginRequestDto dto)
        {
            var auth = await _service.AuthenticateAsync(dto.Email, dto.PasswordHash);
            if (auth == null) return Unauthorized();
            return Ok(auth);
        }

        [HttpPut("{staffId:int}")]
        public async Task<ActionResult<StaffDto>> Update(int staffId, [FromBody] StaffUpdateDto dto)
        {
            var updated = await _service.Update(staffId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{staffId:int}")]
        public async Task<IActionResult> Delete(int staffId)
            => await _service.Delete(staffId) ? NoContent() : NotFound();
    }
}
