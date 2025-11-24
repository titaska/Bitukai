using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Api.BusinessStaff.Models.DTOs;

namespace Pos.Api.BusinessStaff.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDto>> GetAllAsync();
        Task<IEnumerable<StaffDto>> GetByBusinessAsync(string registrationNumber);
        Task<StaffDto?> GetByIdAsync(int staffId);
        Task<StaffDto> CreateAsync(StaffCreateDto dto);
        Task<bool> UpdateAsync(int staffId, StaffUpdateDto dto);
        Task<bool> DeleteAsync(int staffId);
    }
}
