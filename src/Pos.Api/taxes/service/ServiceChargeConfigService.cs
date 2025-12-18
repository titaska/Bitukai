using Pos.Api.Context;
using Pos.Api.taxes.dto;
using Microsoft.EntityFrameworkCore;
using Pos.Api.taxes.model;

namespace Pos.Api.taxes.service;

public interface IServiceChargeConfigService
{
    Task<List<ServiceChargeConfigDto>> GetAllAsync();
    Task<ServiceChargeConfigDto> CreateAsync(ServiceChargeConfigCreateDto dto);
}

public class ServiceChargeConfigService : IServiceChargeConfigService
{
    private readonly AppDbContext _context;

    public ServiceChargeConfigService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceChargeConfigDto>> GetAllAsync()
    {
        return await _context.ServiceChargeConfigs
            .Select(s => new ServiceChargeConfigDto
            {
                ServiceChargeConfigId = s.serviceChargeConfigId,
                RegistrationNumber = s.registrationNumber,
                Percentage = s.percentage,
                ValidFrom = s.validFrom,
                ValidTo = s.validTo
            })
            .ToListAsync();
    }

    public async Task<ServiceChargeConfigDto> CreateAsync(ServiceChargeConfigCreateDto dto)
    {
        var entity = new ServiceChargeConfig
        {
            serviceChargeConfigId = Guid.NewGuid(),
            registrationNumber = dto.RegistrationNumber,
            percentage = dto.Percentage,
            validFrom = dto.ValidFrom,
            validTo = dto.ValidTo
        };

        _context.ServiceChargeConfigs.Add(entity);
        await _context.SaveChangesAsync();

        return new ServiceChargeConfigDto
        {
            ServiceChargeConfigId = entity.serviceChargeConfigId,
            RegistrationNumber = entity.registrationNumber,
            Percentage = entity.percentage,
            ValidFrom = entity.validFrom,
            ValidTo = entity.validTo
        };
    }
}
