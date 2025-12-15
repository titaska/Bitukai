using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.Products.model;

namespace Pos.Api.BusinessStaff.Services
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _db;
        public StaffService(AppDbContext db) => _db = db;

        public async Task<List<StaffDto>> GetAll(string registrationNumber)
        {
            var entities = await _db.Staff
                .Where(s => s.registrationNumber == registrationNumber)
                .ToListAsync();

            return entities.Select(ToDto).ToList();
        }

        public async Task<StaffDto?> GetById(Guid staffId)
        {
            var s = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);
            return s == null ? null : ToDto(s);
        }

        public async Task<StaffDto> Create(StaffCreateDto dto)
        {
            var entity = new Staff
            {
                staffId = Guid.NewGuid(),
                registrationNumber = dto.RegistrationNumber,
                status = dto.Status,
                firstName = dto.FirstName,
                lastName = dto.LastName,
                email = dto.Email,
                phoneNumber = dto.PhoneNumber,
                role = dto.Role,
                hireDate = dto.HireDate,
                Password = dto.Password 
            };

            _db.Staff.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }
        
        public async Task<StaffDto?> AuthenticateAsync(string email, string password)
        {
            var staff = await _db.Staff.FirstOrDefaultAsync(s => s.email == email);
            if (staff == null) return null;
            
            if (staff.Password != password) return null;
            
            return new StaffDto
            {
                StaffId = staff.staffId,
                RegistrationNumber = staff.registrationNumber,
                Status = staff.status,
                FirstName = staff.firstName,
                LastName = staff.lastName,
                Email = staff.email,
                PhoneNumber = staff.phoneNumber,
                Role = staff.role,
                HireDate = staff.hireDate
            };
        }

        public async Task<StaffDto?> Update(Guid staffId, StaffUpdateDto dto)
        {
            var entity = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);
            if (entity == null) return null;

            entity.status = dto.Status;
            entity.firstName = dto.FirstName;
            entity.lastName = dto.LastName;
            entity.email = dto.Email;
            entity.phoneNumber = dto.PhoneNumber;
            entity.role = dto.Role;
            entity.Password = dto.Password; 
            

            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> Delete(Guid staffId)
        {
            var entity = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);
            if (entity == null) return false;

            _db.Staff.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static StaffDto ToDto(Staff s) =>
            new StaffDto
            {
                StaffId = s.staffId,
                RegistrationNumber = s.registrationNumber,
                Status = s.status,
                FirstName = s.firstName,
                LastName = s.lastName,
                Email = s.email,
                PhoneNumber = s.phoneNumber,
                Role = s.role,
                HireDate = s.hireDate,
                Password = s.Password 
            };
    }
}
