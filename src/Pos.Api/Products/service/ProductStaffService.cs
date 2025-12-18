using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pos.Api.Context;
using Pos.Api.Products.dto;
using Pos.Api.Products.model;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.Products.service;

public interface IProductStaffService
{
    Task<IEnumerable<StaffDto>> GetEligibleStaffAsync(Guid productId);
    Task<ProductStaffDto> LinkStaffToProductAsync(Guid productId, ProductStaffCreateDto dto);
    Task<ProductStaffDto?> UpdateProductStaffAsync(Guid productId, Guid staffId, ProductStaffUpdateDto dto);
    Task<bool> UnlinkStaffFromProductAsync(Guid productId, Guid staffId);
}

public class ProductStaffService : IProductStaffService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductStaffService> _logger;

    public ProductStaffService(AppDbContext context, ILogger<ProductStaffService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<StaffDto>> GetEligibleStaffAsync(Guid productId)
    {
        _logger.LogInformation("Fetching eligible staff for product {ProductId}", productId);

        var staffIds = await _context.ProductStaff
            .Where(ps => ps.productId == productId && ps.status)
            .Select(ps => ps.staffId)
            .ToListAsync();

        var staffList = await _context.Staff
            .Where(s => staffIds.Contains(s.staffId) && s.status == StaffStatus.ACTIVE)
            .Select(s => new StaffDto
            {
                StaffId = s.staffId,
                RegistrationNumber = s.registrationNumber,
                Status = s.status,
                FirstName = s.firstName,
                LastName = s.lastName,
                Email = s.email,
                PhoneNumber = s.phoneNumber,
                Role = s.role,
                HireDate = s.hireDate
            })
            .ToListAsync();

        _logger.LogInformation("Found {Count} eligible staff members for product {ProductId}", staffList.Count, productId);

        return staffList;
    }

    public async Task<ProductStaffDto> LinkStaffToProductAsync(Guid productId, ProductStaffCreateDto dto)
    {
        _logger.LogInformation("Linking staff {StaffId} to product {ProductId}", dto.staffId, productId);

        var staffExists = await _context.Staff.AnyAsync(s => s.staffId == dto.staffId && s.status == StaffStatus.ACTIVE);
        if (!staffExists)
        {
            _logger.LogWarning("Staff {StaffId} not found or not active", dto.staffId);
            throw new InvalidOperationException("Staff not found or not active.");
        }

        var productStaff = new ProductStaff
        {
            productStaffId = Guid.NewGuid(),
            productId = productId,
            staffId = dto.staffId,
            status = dto.status,
            valideFrom = dto.validFrom ?? DateTime.UtcNow,
            valideTo = dto.validTo
        };

        _context.ProductStaff.Add(productStaff);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Linked staff {StaffId} to product {ProductId} as ProductStaff {ProductStaffId}", dto.staffId, productId, productStaff.productStaffId);

        return new ProductStaffDto
        {
            productStaffId = productStaff.productStaffId,
            productId = productStaff.productId,
            staffId = productStaff.staffId,
            status = productStaff.status,
            validFrom = productStaff.valideFrom,
            validTo = productStaff.valideTo
        };
    }

    public async Task<ProductStaffDto?> UpdateProductStaffAsync(Guid productId, Guid staffId, ProductStaffUpdateDto dto)
    {
        _logger.LogInformation("Updating ProductStaff link for product {ProductId} and staff {StaffId}", productId, staffId);

        var productStaff = await _context.ProductStaff
            .FirstOrDefaultAsync(ps => ps.productId == productId && ps.staffId == staffId);

        if (productStaff == null)
        {
            _logger.LogWarning("ProductStaff link not found for product {ProductId} and staff {StaffId}", productId, staffId);
            return null;
        }

        productStaff.status = dto.status;
        productStaff.valideFrom = dto.validFrom ?? productStaff.valideFrom;
        productStaff.valideTo = dto.validTo ?? productStaff.valideTo;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated ProductStaff link {ProductStaffId} for product {ProductId} and staff {StaffId}", productStaff.productStaffId, productId, staffId);

        return new ProductStaffDto
        {
            productStaffId = productStaff.productStaffId,
            productId = productStaff.productId,
            staffId = productStaff.staffId,
            status = productStaff.status,
            validFrom = productStaff.valideFrom,
            validTo = productStaff.valideTo
        };
    }

    public async Task<bool> UnlinkStaffFromProductAsync(Guid productId, Guid staffId)
    {
        _logger.LogInformation("Unlinking staff {StaffId} from product {ProductId}", staffId, productId);

        var productStaff = await _context.ProductStaff
            .FirstOrDefaultAsync(ps => ps.productId == productId && ps.staffId == staffId);

        if (productStaff == null)
        {
            _logger.LogWarning("No ProductStaff link found for product {ProductId} and staff {StaffId}", productId, staffId);
            return false;
        }

        _context.ProductStaff.Remove(productStaff);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Unlinked staff {StaffId} from product {ProductId} (ProductStaff {ProductStaffId})", staffId, productId, productStaff.productStaffId);

        return true;
    }
}
