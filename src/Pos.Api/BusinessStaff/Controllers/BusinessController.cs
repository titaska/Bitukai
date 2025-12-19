using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Services;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/business")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IBusinessService businessService, ILogger<BusinessController> logger)
        {
            _businessService = businessService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all businesses");
            var businesses = await _businessService.GetAllAsync();
            _logger.LogInformation("Fetched {Count} businesses", businesses.Count());
            return Ok(businesses);
        }

        [HttpGet("{registrationNumber}")]
        public async Task<ActionResult<BusinessDto>> Get(string registrationNumber)
        {
            _logger.LogInformation("Fetching business with registrationNumber: {RegistrationNumber}", registrationNumber);
            var business = await _businessService.GetByIdAsync(registrationNumber);
            if (business == null)
            {
                _logger.LogWarning("Business not found: {RegistrationNumber}", registrationNumber);
                return NotFound();
            }
            _logger.LogInformation("Fetched business: {RegistrationNumber}", registrationNumber);
            return Ok(business);
        }

        [HttpPost]
        public async Task<ActionResult<BusinessDto>> Create([FromBody] BusinessCreateDto dto)
        {
            _logger.LogInformation("Creating new business");
            var created = await _businessService.CreateAsync(dto);
            _logger.LogInformation("Created business: {RegistrationNumber}", created.RegistrationNumber);

            return CreatedAtAction(nameof(Get),
                new { registrationNumber = created.RegistrationNumber }, created);
        }

        [HttpPut("{registrationNumber}")]
        public async Task<IActionResult> Update(string registrationNumber, [FromBody] BusinessUpdateDto dto)
        {
            _logger.LogInformation("Updating business: {RegistrationNumber}", registrationNumber);
            var success = await _businessService.UpdateAsync(registrationNumber, dto);
            if (!success)
            {
                _logger.LogWarning("Failed to update business (not found): {RegistrationNumber}", registrationNumber);
                return NotFound();
            }
            _logger.LogInformation("Updated business successfully: {RegistrationNumber}", registrationNumber);
            return NoContent();
        }

        [HttpDelete("{registrationNumber}")]
        public async Task<IActionResult> Delete(string registrationNumber)
        {
            _logger.LogInformation("Deleting business: {RegistrationNumber}", registrationNumber);
            var success = await _businessService.DeleteAsync(registrationNumber);
            if (!success)
            {
                _logger.LogWarning("Failed to delete business (not found): {RegistrationNumber}", registrationNumber);
                return NotFound();
            }
            _logger.LogInformation("Deleted business successfully: {RegistrationNumber}", registrationNumber);
            return NoContent();
        }
    }
}
