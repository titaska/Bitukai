using Microsoft.AspNetCore.Mvc;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Services;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/services/{productId}/staff")]
    public class ServiceStaffController : ControllerBase
    {
        private readonly IAssignmentService _service;
        public ServiceStaffController(IAssignmentService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> GetStaff(Guid productId)
            => Ok(await _service.GetStaffForService(productId));

        [HttpPost]
        public async Task<ActionResult<ProductStaffDto>> Assign(Guid productId, [FromBody] AssignStaffToServiceDto dto)
            => Ok(await _service.Assign(productId, dto));

        [HttpDelete("{staffId:int}")]
        public async Task<IActionResult> Unassign(Guid productId, int staffId)
            => await _service.Unassign(productId, staffId) ? NoContent() : NotFound();
    }
}
