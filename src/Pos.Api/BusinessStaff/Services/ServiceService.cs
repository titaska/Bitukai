using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Services
{
    public class ServiceService : IServiceService
    {
        private readonly AppDbContext _db;
        public ServiceService(AppDbContext db) => _db = db;

        public async Task<List<ServiceDto>> GetAll(string registrationNumber)
        {
            var entities = await _db.Services
                .Where(p => p.registrationNumber == registrationNumber && p.type == ServiceType.SERVICE)
                .ToListAsync();

            return entities.Select(ToDto).ToList();
        }

        public async Task<ServiceDto?> GetById(Guid serviceId)
        {
            var p = await _db.Services
                .FirstOrDefaultAsync(x => x.serviceId == serviceId && x.type == ServiceType.SERVICE);

            return p == null ? null : ToDto(p);
        }

        public async Task<ServiceDto> Create(ServiceCreateDto dto)
        {
            var entity = new Service
            {
                serviceId = dto.ServiceId == Guid.Empty ? Guid.NewGuid() : dto.ServiceId,
                registrationNumber = dto.RegistrationNumber,
                type = ServiceType.SERVICE,
                name = dto.Name,
                description = dto.Description,
                basePrice = dto.BasePrice,
                taxCode = dto.TaxCode,
                durationMinutes = dto.DurationMinutes,
                status = dto.Status
            };

            _db.Services.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<ServiceDto?> Update(Guid serviceId, ServiceUpdateDto dto)
        {
            var entity = await _db.Services
                .FirstOrDefaultAsync(x => x.serviceId == serviceId && x.type == ServiceType.SERVICE);

            if (entity == null) return null;

            entity.name = dto.Name;
            entity.description = dto.Description;
            entity.basePrice = dto.BasePrice;
            entity.taxCode = dto.TaxCode;
            entity.durationMinutes = dto.DurationMinutes;
            entity.status = dto.Status;

            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> Delete(Guid serviceId)
        {
            var entity = await _db.Services
                .FirstOrDefaultAsync(x => x.serviceId == serviceId && x.type == ServiceType.SERVICE);

            if (entity == null) return false;

            _db.Services.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static ServiceDto ToDto(Service p) =>
            new ServiceDto
            {
                ServiceId = p.serviceId,
                RegistrationNumber = p.registrationNumber,
                Type = p.type.ToString(),
                Name = p.name,
                Description = p.description,
                BasePrice = p.basePrice,
                TaxCode = p.taxCode,
                Status = p.status,
                DurationMinutes = p.durationMinutes
            };
    }
}
