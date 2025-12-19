using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.Products.model;

namespace Pos.Api.BusinessStaff.Services
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<StaffService> _logger;

        public StaffService(AppDbContext db, ILogger<StaffService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<StaffDto>> GetAll(string registrationNumber)
        {
            _logger.LogInformation("Fetching all staff for business: {RegistrationNumber}", registrationNumber);
            var entities = await _db.Staff
                .Where(s => s.registrationNumber == registrationNumber)
                .ToListAsync();

            _logger.LogInformation("Found {Count} staff members for business: {RegistrationNumber}", entities.Count, registrationNumber);
            return entities.Select(ToDto).ToList();
        }

        public async Task<StaffDto?> GetById(Guid staffId)
        {
            _logger.LogInformation("Fetching staff by ID: {StaffId}", staffId);
            var s = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);

            if (s == null)
            {
                _logger.LogWarning("Staff not found: {StaffId}", staffId);
                return null;
            }

            _logger.LogInformation("Found staff: {StaffId}", staffId);
            return ToDto(s);
        }

        public async Task<StaffDto> Create(StaffCreateDto dto)
        {
            _logger.LogInformation("Creating staff for business: {RegistrationNumber}, Name: {FirstName} {LastName}", dto.RegistrationNumber, dto.FirstName, dto.LastName);

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

            _logger.LogInformation("Created staff: {StaffId}", entity.staffId);
            return ToDto(entity);
        }

        public async Task<StaffDto?> AuthenticateAsync(string email, string password)
        {
            _logger.LogInformation("Authenticating staff with email: {Email}", email);
            var staff = await _db.Staff.FirstOrDefaultAsync(s => s.email == email);

            if (staff == null)
            {
                _logger.LogWarning("Authentication failed, staff not found: {Email}", email);
                return null;
            }

            if (staff.Password != password)
            {
                _logger.LogWarning("Authentication failed, incorrect password for staff: {Email}", email);
                return null;
            }

            _logger.LogInformation("Authentication successful for staff: {StaffId}", staff.staffId);
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
            _logger.LogInformation("Updating staff: {StaffId}", staffId);
            var entity = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);
            if (entity == null)
            {
                _logger.LogWarning("Staff not found for update: {StaffId}", staffId);
                return null;
            }

            entity.status = dto.Status;
            entity.firstName = dto.FirstName;
            entity.lastName = dto.LastName;
            entity.email = dto.Email;
            entity.phoneNumber = dto.PhoneNumber;
            entity.role = dto.Role;
            entity.Password = dto.Password;

            await _db.SaveChangesAsync();
            _logger.LogInformation("Updated staff: {StaffId}", staffId);
            return ToDto(entity);
        }

        public async Task<bool> Delete(Guid staffId)
        {
            _logger.LogInformation("Deleting staff: {StaffId}", staffId);
            var entity = await _db.Staff.FirstOrDefaultAsync(x => x.staffId == staffId);
            if (entity == null)
            {
                _logger.LogWarning("Staff not found for deletion: {StaffId}", staffId);
                return false;
            }

            _db.Staff.Remove(entity);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Deleted staff successfully: {StaffId}", staffId);
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
