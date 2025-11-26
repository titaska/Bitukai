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
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
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
            var entity = new Staff
            {
                RegistrationNumber = dto.RegistrationNumber,
                Status = dto.Status,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = dto.PasswordHash,
                Role = dto.Role,
                HireDate = dto.HireDate
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
            entity.HireDate = dto.HireDate;

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
