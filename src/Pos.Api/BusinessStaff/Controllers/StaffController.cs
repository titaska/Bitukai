using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.BusinessStaff.Models.DTOs;
using Pos.Api.BusinessStaff.Services.Interfaces;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            var staff = await _staffService.GetAllAsync();
            return Ok(staff);
        }

        [HttpGet("{staffId:int}")]
        public async Task<ActionResult<StaffDto>> Get(int staffId)
        {
            var staff = await _staffService.GetByIdAsync(staffId);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        // Staff pagal business registrationNumber
        [HttpGet("by-business/{registrationNumber}")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetByBusiness(string registrationNumber)
        {
            var staff = await _staffService.GetByBusinessAsync(registrationNumber);
            return Ok(staff);
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create([FromBody] StaffCreateDto dto)
        {
            var created = await _staffService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { staffId = created.StaffId }, created);
        }

        [HttpPut("{staffId:int}")]
        public async Task<IActionResult> Update(int staffId, [FromBody] StaffUpdateDto dto)
        {
            var success = await _staffService.UpdateAsync(staffId, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{staffId:int}")]
        public async Task<IActionResult> Delete(int staffId)
        {
            var success = await _staffService.DeleteAsync(staffId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
