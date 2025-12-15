using Pos.Api.BusinessStaff.dto;

namespace Pos.Api.BusinessStaff.Services
{
    public interface IAssignmentService
    {
        Task<List<StaffDto>> GetStaffForService(Guid serviceId);
        Task<ServiceStaffDto> Assign(Guid serviceId, AssignStaffToServiceDto dto);
        Task<bool> Unassign(Guid serviceId, int staffId);
    }
}
