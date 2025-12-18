using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.taxes.dto;
using Pos.Api.taxes.service;

namespace Pos.Api.taxes.controller
{
    [ApiController]
    [Route("api/tax")]
    public class TaxesController : ControllerBase
    {
        private readonly TaxService _service;
        private readonly IServiceChargeConfigService _serviceChargeConfigService;
        private readonly ILogger<TaxesController> _logger;

        public TaxesController(
            TaxService service,  
            IServiceChargeConfigService serviceChargeConfigService,
            ILogger<TaxesController> logger)
        {
            _service = service;
            _serviceChargeConfigService = serviceChargeConfigService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all taxes");
            var taxes = await _service.GetAllAsync();
            _logger.LogInformation("Fetched {Count} taxes", taxes.Count);
            return Ok(taxes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("Fetching tax with ID {TaxId}", id);
            var tax = await _service.GetByIdAsync(id);
            if (tax == null)
            {
                _logger.LogWarning("Tax not found: {TaxId}", id);
                return NotFound();
            }
            _logger.LogInformation("Found tax {TaxId}", id);
            return Ok(tax);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxCreateDto dto)
        {
            _logger.LogInformation("Creating new tax: {@TaxDto}", dto);
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Created tax with ID {TaxId}", created.id);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TaxCreateDto dto)
        {
            _logger.LogInformation("Updating tax {TaxId} with {@TaxDto}", id, dto);
            await _service.UpdateAsync(id, dto);
            _logger.LogInformation("Updated tax {TaxId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting tax {TaxId}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Deleted tax {TaxId}", id);
            return NoContent();
        }
        
        [HttpGet("service-charges")]
        public async Task<IActionResult> GetServiceCharges()
        {
            _logger.LogInformation("Fetching all service charge configs");
            var serviceCharges = await _serviceChargeConfigService.GetAllAsync();
            _logger.LogInformation("Fetched {Count} service charge configs", serviceCharges.Count);
            return Ok(serviceCharges);
        }
        
        [HttpPost("service-charges")]
        public async Task<IActionResult> CreateServiceCharge([FromBody] ServiceChargeConfigCreateDto dto)
        {
            _logger.LogInformation("Creating service charge config: {@ServiceChargeDto}", dto);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid service charge config DTO: {@ServiceChargeDto}", dto);
                return BadRequest(ModelState);
            }

            var created = await _serviceChargeConfigService.CreateAsync(dto);
            _logger.LogInformation("Created service charge config with ID {ConfigId}", created.ServiceChargeConfigId);

            return CreatedAtAction(nameof(GetServiceCharges), new { id = created.ServiceChargeConfigId }, created);
        }
    }
}

