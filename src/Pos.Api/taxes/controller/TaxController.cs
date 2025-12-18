using Microsoft.AspNetCore.Mvc;
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

        public TaxesController(TaxService service,  IServiceChargeConfigService serviceChargeConfigService)
        {
            _service = service;
            _serviceChargeConfigService = serviceChargeConfigService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tax = await _service.GetByIdAsync(id);
            return tax == null ? NotFound() : Ok(tax);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TaxCreateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        
        [HttpGet("service-charges")]
        public async Task<IActionResult> GetServiceCharges()
        {
            var serviceCharges = await _serviceChargeConfigService.GetAllAsync();
            return Ok(serviceCharges);
        }
        
        [HttpPost("service-charges")]
        public async Task<IActionResult> CreateServiceCharge([FromBody] ServiceChargeConfigCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _serviceChargeConfigService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetServiceCharges), new { id = created.ServiceChargeConfigId }, created);
        }
    }
}

