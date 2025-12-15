using Pos.Api.BusinessStaff.dto;

namespace Pos.Api.BusinessStaff.Services
{
    public interface IStaffService
    {
        Task<List<StaffDto>> GetAll(string registrationNumber);
        Task<StaffDto?> GetById(int staffId);
        Task<StaffDto> Create(StaffCreateDto dto);
        Task<StaffDto?> Update(int staffId, StaffUpdateDto dto);
        Task<bool> Delete(int staffId);
    }
}
