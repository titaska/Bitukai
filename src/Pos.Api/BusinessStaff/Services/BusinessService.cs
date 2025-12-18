using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.dto;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BusinessService> _logger;

        public BusinessService(AppDbContext context, ILogger<BusinessService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BusinessDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all businesses from database");
            var businesses = await _context.Businesses
                .Select(b => new BusinessDto
                {
                    RegistrationNumber = b.RegistrationNumber,
                    VatCode = b.VatCode,
                    Name = b.Name,
                    Location = b.Location,
                    Phone = b.Phone,
                    Email = b.Email,
                    CurrencyCode = b.CurrencyCode,
                    Type = b.Type
                })
                .ToListAsync();
            _logger.LogInformation("Fetched {Count} businesses", businesses.Count);
            return businesses;
        }

        public async Task<BusinessDto?> GetByIdAsync(string registrationNumber)
        {
            _logger.LogInformation("Fetching business with RegistrationNumber: {RegistrationNumber}", registrationNumber);
            var b = await _context.Businesses.FindAsync(registrationNumber);
            if (b == null)
            {
                _logger.LogWarning("Business not found: {RegistrationNumber}", registrationNumber);
                return null;
            }

            _logger.LogInformation("Fetched business: {RegistrationNumber}", registrationNumber);
            return new BusinessDto
            {
                RegistrationNumber = b.RegistrationNumber,
                VatCode = b.VatCode,
                Name = b.Name,
                Location = b.Location,
                Phone = b.Phone,
                Email = b.Email,
                CurrencyCode = b.CurrencyCode,
                Type = b.Type
            };
        }

        public async Task<BusinessDto> CreateAsync(BusinessCreateDto dto)
        {
            _logger.LogInformation("Creating new business with Name: {Name}", dto.Name);
            var entity = new Business
            {
                RegistrationNumber = Guid.NewGuid().ToString(),
                VatCode = dto.VatCode,
                Name = dto.Name,
                Location = dto.Location,
                Phone = dto.Phone,
                Email = dto.Email,
                CurrencyCode = dto.CurrencyCode,
                Type = dto.Type
            };

            _context.Businesses.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created business: {RegistrationNumber}", entity.RegistrationNumber);
            return await GetByIdAsync(entity.RegistrationNumber)
                   ?? throw new Exception("Failed to create business");
        }

        public async Task<bool> UpdateAsync(string registrationNumber, BusinessUpdateDto dto)
        {
            _logger.LogInformation("Updating business: {RegistrationNumber}", registrationNumber);
            var entity = await _context.Businesses.FindAsync(registrationNumber);
            if (entity == null)
            {
                _logger.LogWarning("Business not found for update: {RegistrationNumber}", registrationNumber);
                return false;
            }

            entity.VatCode = dto.VatCode;
            entity.Name = dto.Name;
            entity.Location = dto.Location;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.CurrencyCode = dto.CurrencyCode;
            entity.Type = dto.Type;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated business successfully: {RegistrationNumber}", registrationNumber);
            return true;
        }

        public async Task<bool> DeleteAsync(string registrationNumber)
        {
            _logger.LogInformation("Deleting business: {RegistrationNumber}", registrationNumber);
            var entity = await _context.Businesses.FindAsync(registrationNumber);
            if (entity == null)
            {
                _logger.LogWarning("Business not found for deletion: {RegistrationNumber}", registrationNumber);
                return false;
            }

            _context.Businesses.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted business successfully: {RegistrationNumber}", registrationNumber);
            return true;
        }
    }
}
