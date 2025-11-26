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
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _context;

        public BusinessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessDto>> GetAllAsync()
        {
            return await _context.Businesses
                .Select(b => new BusinessDto
                {
                    RegistrationNumber = b.RegistrationNumber,
                    VatCode = b.VatCode,
                    Name = b.Name,
                    Location = b.Location,
                    Phone = b.Phone,
                    Email = b.Email,
                    CurrencyCode = b.CurrencyCode
                })
                .ToListAsync();
        }

        public async Task<BusinessDto?> GetByIdAsync(string registrationNumber)
        {
            var b = await _context.Businesses.FindAsync(registrationNumber);
            if (b == null) return null;

            return new BusinessDto
            {
                RegistrationNumber = b.RegistrationNumber,
                VatCode = b.VatCode,
                Name = b.Name,
                Location = b.Location,
                Phone = b.Phone,
                Email = b.Email,
                CurrencyCode = b.CurrencyCode
            };
        }

        public async Task<BusinessDto> CreateAsync(BusinessCreateDto dto)
        {
            var entity = new Business
            {
                RegistrationNumber = dto.RegistrationNumber,
                VatCode = dto.VatCode,
                Name = dto.Name,
                Location = dto.Location,
                Phone = dto.Phone,
                Email = dto.Email,
                CurrencyCode = dto.CurrencyCode
            };

            _context.Businesses.Add(entity);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(entity.RegistrationNumber)
                   ?? throw new Exception("Failed to create business");
        }

        public async Task<bool> UpdateAsync(string registrationNumber, BusinessUpdateDto dto)
        {
            var entity = await _context.Businesses.FindAsync(registrationNumber);
            if (entity == null) return false;

            entity.VatCode = dto.VatCode;
            entity.Name = dto.Name;
            entity.Location = dto.Location;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.CurrencyCode = dto.CurrencyCode;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string registrationNumber)
        {
            var entity = await _context.Businesses.FindAsync(registrationNumber);
            if (entity == null) return false;

            _context.Businesses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
