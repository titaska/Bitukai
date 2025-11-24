using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Api.BusinessStaff.Models.DTOs;

namespace Pos.Api.BusinessStaff.Services.Interfaces
{
    public interface IBusinessService
    {
        Task<IEnumerable<BusinessDto>> GetAllAsync();
        Task<BusinessDto?> GetByIdAsync(string registrationNumber);
        Task<BusinessDto> CreateAsync(BusinessCreateDto dto);
        Task<bool> UpdateAsync(string registrationNumber, BusinessUpdateDto dto);
        Task<bool> DeleteAsync(string registrationNumber);
    }
}
