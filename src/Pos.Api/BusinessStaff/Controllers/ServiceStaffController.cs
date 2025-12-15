using Microsoft.AspNetCore.Mvc;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Services;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/services/{serviceId}/staff")]
    public class ServiceStaffController : ControllerBase
    {
        private readonly IAssignmentService _service;
        public ServiceStaffController(IAssignmentService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> GetStaff(Guid serviceId)
            => Ok(await _service.GetStaffForService(serviceId));

        [HttpPost]
        public async Task<ActionResult<ServiceStaffDto>> Assign(Guid serviceId, [FromBody] AssignStaffToServiceDto dto)
            => Ok(await _service.Assign(serviceId, dto));

        [HttpDelete("{staffId:int}")]
        public async Task<IActionResult> Unassign(Guid serviceId, int staffId)
            => await _service.Unassign(serviceId, staffId) ? NoContent() : NotFound();
    }
}
