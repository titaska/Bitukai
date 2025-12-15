using Pos.Api.BusinessStaff.dto;
using Pos.Api.Products.model;

namespace Pos.Api.BusinessStaff.Services
{
    public interface IStaffService
    {
        Task<List<StaffDto>> GetAll(string registrationNumber);
        Task<StaffDto?> GetById(Guid staffId);
        Task<StaffDto> Create(StaffCreateDto dto);
        Task<StaffDto?> Update(Guid staffId, StaffUpdateDto dto);
        Task<StaffDto?> AuthenticateAsync(string email, string password);
        Task<bool> Delete(Guid staffId);
    }
}
