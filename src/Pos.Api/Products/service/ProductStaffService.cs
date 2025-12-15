using Microsoft.EntityFrameworkCore;

namespace Pos.Api.Products.service;
using Pos.Api.Context;
using Pos.Api.Products.dto;
using Pos.Api.Products.model;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;

public interface IProductStaffService
{
    Task<IEnumerable<StaffDto>> GetEligibleStaffAsync(Guid productId);
    Task<ProductStaffDto> LinkStaffToProductAsync(Guid productId, ProductStaffCreateDto dto);
    Task<ProductStaffDto?> UpdateProductStaffAsync(Guid productId, int staffId, ProductStaffUpdateDto dto);
    Task<bool> UnlinkStaffFromProductAsync(Guid productId, int staffId);
}

public class ProductStaffService : IProductStaffService
{
    private readonly AppDbContext _context;

    public ProductStaffService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffDto>> GetEligibleStaffAsync(Guid productId)
    {
        // Step 1: Get all active ProductStaff entries for the given product
        var staffIds = await _context.ProductStaff
            .Where(ps => ps.productId == productId && ps.status) // only active ProductStaff
            .Select(ps => ps.staffId)
            .ToListAsync();

        // Step 2: Get all Staff entities that match those staffIds and are ACTIVE
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

        return staffList;
    }
    
    public async Task<ProductStaffDto> LinkStaffToProductAsync(Guid productId, ProductStaffCreateDto dto)
    {
        var staffExists = await _context.Staff.AnyAsync(s => s.staffId == dto.staffId && s.status == StaffStatus.ACTIVE);
        if (!staffExists)
        {
            throw new InvalidOperationException("Staff not found or not active.");
        }

        // Create new ProductStaff entry
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

        // Return as DTO
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
    
    public async Task<ProductStaffDto?> UpdateProductStaffAsync(Guid productId, int staffId, ProductStaffUpdateDto dto)
    {
        // Find existing ProductStaff entry
        var productStaff = await _context.ProductStaff
            .FirstOrDefaultAsync(ps => ps.productId == productId && ps.staffId == staffId);

        if (productStaff == null)
            return null;

        // Update fields
        productStaff.status = dto.status;
        productStaff.valideFrom = dto.validFrom ?? productStaff.valideFrom;
        productStaff.valideTo = dto.validTo ?? productStaff.valideTo;

        await _context.SaveChangesAsync();

        // Return updated DTO
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
    
    public async Task<bool> UnlinkStaffFromProductAsync(Guid productId, int staffId)
    {
        // Find existing ProductStaff entry
        var productStaff = await _context.ProductStaff
            .FirstOrDefaultAsync(ps => ps.productId == productId && ps.staffId == staffId);

        if (productStaff == null)
            return false;

        _context.ProductStaff.Remove(productStaff);
        await _context.SaveChangesAsync();

        return true;
    }
}
