using Pos.Api.BusinessStaff.dto;

namespace Pos.Api.BusinessStaff.Services
{
    public interface IAssignmentService
    {
        Task<List<StaffDto>> GetStaffForService(Guid productId);
        Task<ProductStaffDto> Assign(Guid productId, AssignStaffToServiceDto dto);
        Task<bool> Unassign(Guid productId, int staffId);
    }
}
