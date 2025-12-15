using Microsoft.AspNetCore.Mvc;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Services;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _service;
        public ServicesController(IServiceService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<ServiceDto>>> GetAll([FromQuery] string registrationNumber)
            => Ok(await _service.GetAll(registrationNumber));

        [HttpGet("{serviceId}")]
        public async Task<ActionResult<ServiceDto>> GetById(Guid serviceId)
        {
            var item = await _service.GetById(serviceId);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceCreateDto dto)
        {
            var created = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { serviceId = created.ServiceId }, created);
        }

        [HttpPut("{serviceId}")]
        public async Task<ActionResult<ServiceDto>> Update(Guid serviceId, [FromBody] ServiceUpdateDto dto)
        {
            var updated = await _service.Update(serviceId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> Delete(Guid serviceId)
            => await _service.Delete(serviceId) ? NoContent() : NotFound();
    }
}
