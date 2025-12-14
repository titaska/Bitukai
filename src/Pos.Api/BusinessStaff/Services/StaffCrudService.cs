using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.Models.DTOs;
using Pos.Api.BusinessStaff.Services.Interfaces;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Services
{
    public class StaffCrudService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffCrudService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StaffDto>> GetAllAsync()
        {
            return await _context.Staff
                .Select(s => new StaffDto
                {
                    StaffId = s.StaffId,
                    RegistrationNumber = s.RegistrationNumber,
                    Status = s.Status,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Role = s.Role,
                    HireDate = s.HireDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<StaffDto>> GetByBusinessAsync(string registrationNumber)
        {
            return await _context.Staff
                .Where(s => s.RegistrationNumber == registrationNumber)
                .Select(s => new StaffDto
                {
                    StaffId = s.StaffId,
                    RegistrationNumber = s.RegistrationNumber,
                    Status = s.Status,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Role = s.Role,
                    HireDate = s.HireDate
                })
                .ToListAsync();
        }

        public async Task<StaffDto?> GetByIdAsync(int staffId)
        {
            var s = await _context.Staff.FindAsync(staffId);
            if (s == null) return null;

            return new StaffDto
            {
                StaffId = s.StaffId,
                RegistrationNumber = s.RegistrationNumber,
                Status = s.Status,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Role = s.Role,
                HireDate = s.HireDate
            };
        }

        public async Task<StaffDto> CreateAsync(StaffCreateDto dto)
        {
            var defaultBusiness = await _context.Businesses.FirstOrDefaultAsync();
            if (defaultBusiness == null)
            {
                throw new InvalidOperationException(
                    "No Business found in database. Cannot create staff without associated Business.");
            }

            var entity = new Staff
            {
                RegistrationNumber = defaultBusiness.RegistrationNumber, // automatiškai
                Status = dto.Status,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = dto.PasswordHash,
                Role = dto.Role,
                HireDate = DateTime.SpecifyKind(dto.HireDate, DateTimeKind.Utc)
            };

            _context.Staff.Add(entity);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(entity.StaffId)
                   ?? throw new Exception("Failed to create staff");
        }

        public async Task<bool> UpdateAsync(int staffId, StaffUpdateDto dto)
        {
            var entity = await _context.Staff.FindAsync(staffId);
            if (entity == null) return false;

            entity.Status = dto.Status;
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
            {
                entity.PasswordHash = dto.PasswordHash;
            }

            entity.Role = dto.Role;
            entity.HireDate = DateTime.SpecifyKind(dto.HireDate, DateTimeKind.Utc);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int staffId)
        {
            var entity = await _context.Staff.FindAsync(staffId);
            if (entity == null) return false;

            _context.Staff.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
