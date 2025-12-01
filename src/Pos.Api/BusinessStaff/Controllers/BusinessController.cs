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
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessDto>>> GetAll()
        {
            var businesses = await _businessService.GetAllAsync();
            return Ok(businesses);
        }

        [HttpGet("{registrationNumber}")]
        public async Task<ActionResult<BusinessDto>> Get(string registrationNumber)
        {
            var business = await _businessService.GetByIdAsync(registrationNumber);
            if (business == null) return NotFound();
            return Ok(business);
        }

        [HttpPost]
        public async Task<ActionResult<BusinessDto>> Create([FromBody] BusinessCreateDto dto)
        {
            var created = await _businessService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get),
                new { registrationNumber = created.RegistrationNumber }, created);
        }

        [HttpPut("{registrationNumber}")]
        public async Task<IActionResult> Update(string registrationNumber, [FromBody] BusinessUpdateDto dto)
        {
            var success = await _businessService.UpdateAsync(registrationNumber, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{registrationNumber}")]
        public async Task<IActionResult> Delete(string registrationNumber)
        {
            var success = await _businessService.DeleteAsync(registrationNumber);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
