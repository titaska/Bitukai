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

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceDto>> GetById(Guid productId)
        {
            var item = await _service.GetById(productId);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceCreateDto dto)
        {
            var created = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { productId = created.ProductId }, created);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ServiceDto>> Update(Guid productId, [FromBody] ServiceUpdateDto dto)
        {
            var updated = await _service.Update(productId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
            => await _service.Delete(productId) ? NoContent() : NotFound();
    }
}
