using Pos.Api.BusinessStaff.dto;

namespace Pos.Api.BusinessStaff.Services
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetAll(string registrationNumber);
        Task<ServiceDto?> GetById(Guid productId);
        Task<ServiceDto> Create(ServiceCreateDto dto);
        Task<ServiceDto?> Update(Guid productId, ServiceUpdateDto dto);
        Task<bool> Delete(Guid productId);
    }
}
