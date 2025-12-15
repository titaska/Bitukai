using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _db;
        public AssignmentService(AppDbContext db) => _db = db;

        public async Task<List<StaffDto>> GetStaffForService(Guid serviceId)
        {
            return await _db.ServiceStaff
                .Where(ps => ps.serviceId == serviceId && ps.status)
                .Include(ps => ps.staff)
                .Select(ps => new StaffDto
                {
                    StaffId = ps.staff!.staffId,
                    RegistrationNumber = ps.staff.registrationNumber,
                    Status = ps.staff.status,
                    FirstName = ps.staff.firstName,
                    LastName = ps.staff.lastName,
                    Email = ps.staff.email,
                    PhoneNumber = ps.staff.phoneNumber,
                    Role = ps.staff.role,
                    HireDate = ps.staff.hireDate
                })
                .ToListAsync();
        }

        public async Task<ServiceStaffDto> Assign(Guid serviceId, AssignStaffToServiceDto dto)
        {
            if (dto.ValidFrom.HasValue && dto.ValidTo.HasValue && dto.ValidTo.Value < dto.ValidFrom.Value)
                throw new InvalidOperationException("ValidTo negali būti ankstesnis už ValidFrom.");

            var service = await _db.Services
                .FirstOrDefaultAsync(p => p.serviceId == serviceId && p.type == ServiceType.SERVICE);
            if (service == null) throw new KeyNotFoundException("Service (service) nerastas.");

            var staff = await _db.Staff
                .FirstOrDefaultAsync(s => s.staffId == dto.StaffId);
            if (staff == null) throw new KeyNotFoundException("Staff nerastas.");

            if (staff.registrationNumber != service.registrationNumber)
                throw new InvalidOperationException("Negalima priskirti staff iš kito business.");

            var existing = await _db.ServiceStaff
                .FirstOrDefaultAsync(x => x.serviceId == serviceId && x.staffId == dto.StaffId);

            if (existing != null)
            {
                existing.status = dto.Status;
                existing.valideFrom = dto.ValidFrom;
                existing.valideTo = dto.ValidTo;
                await _db.SaveChangesAsync();
                return ToDto(existing);
            }

            var entity = new ServiceStaff
            {
                serviceId = serviceId,
                staffId = dto.StaffId,
                status = dto.Status,
                valideFrom = dto.ValidFrom,
                valideTo = dto.ValidTo
            };

            _db.ServiceStaff.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> Unassign(Guid serviceId, int staffId)
        {
            var entity = await _db.ServiceStaff
                .FirstOrDefaultAsync(x => x.serviceId == serviceId && x.staffId == staffId);

            if (entity == null) return false;

            _db.ServiceStaff.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static ServiceStaffDto ToDto(ServiceStaff ps) =>
            new ServiceStaffDto
            {
                ServiceStaffId = ps.serviceStaffId,
                ServiceId = ps.serviceId,
                StaffId = ps.staffId,
                Status = ps.status,
                ValideFrom = ps.valideFrom,
                ValideTo = ps.valideTo
            };
    }
}
