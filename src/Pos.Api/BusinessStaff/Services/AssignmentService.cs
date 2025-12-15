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

        public async Task<List<StaffDto>> GetStaffForService(Guid productId)
        {
            return await _db.ProductStaff
                .Where(ps => ps.productId == productId && ps.status)
                .Include(ps => ps.staff)
                .Select(ps => new StaffDto
                {
                    StaffId = ps.staff!.staffId,
                    RegistrationNumber = ps.staff.registrationNumber,
                    Status = ps.staff.status,
                    FirstName = ps.staff.firstName,
                    LastName = ps.staff.lastName,
                    Email = ps.staff.email,
                    PhoneNumber = ps.staff.phoneNumber
                })
                .ToListAsync();
        }

        public async Task<ProductStaffDto> Assign(Guid productId, AssignStaffToServiceDto dto)
        {
            if (dto.ValidFrom.HasValue && dto.ValidTo.HasValue && dto.ValidTo.Value < dto.ValidFrom.Value)
                throw new InvalidOperationException("ValidTo negali būti ankstesnis už ValidFrom.");

            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.productId == productId && p.type == ProductType.SERVICE);
            if (product == null) throw new KeyNotFoundException("Service (Product) nerastas.");

            var staff = await _db.Staff
                .FirstOrDefaultAsync(s => s.staffId == dto.StaffId);
            if (staff == null) throw new KeyNotFoundException("Staff nerastas.");

            if (staff.registrationNumber != product.registrationNumber)
                throw new InvalidOperationException("Negalima priskirti staff iš kito business.");

            var existing = await _db.ProductStaff
                .FirstOrDefaultAsync(x => x.productId == productId && x.staffId == dto.StaffId);

            if (existing != null)
            {
                existing.status = dto.Status;
                existing.valideFrom = dto.ValidFrom;
                existing.valideTo = dto.ValidTo;
                await _db.SaveChangesAsync();
                return ToDto(existing);
            }

            var entity = new ProductStaff
            {
                productId = productId,
                staffId = dto.StaffId,
                status = dto.Status,
                valideFrom = dto.ValidFrom,
                valideTo = dto.ValidTo
            };

            _db.ProductStaff.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> Unassign(Guid productId, int staffId)
        {
            var entity = await _db.ProductStaff
                .FirstOrDefaultAsync(x => x.productId == productId && x.staffId == staffId);

            if (entity == null) return false;

            _db.ProductStaff.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static ProductStaffDto ToDto(ProductStaff ps) =>
            new ProductStaffDto
            {
                ProductStaffId = ps.productStaffId,
                ProductId = ps.productId,
                StaffId = ps.staffId,
                Status = ps.status,
                ValideFrom = ps.valideFrom,
                ValideTo = ps.valideTo
            };
    }
}
