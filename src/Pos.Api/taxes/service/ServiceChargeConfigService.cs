using Pos.Api.Context;
using Pos.Api.taxes.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ServiceChargeConfigService> _logger;

    public ServiceChargeConfigService(AppDbContext context, ILogger<ServiceChargeConfigService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ServiceChargeConfigDto>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all service charge configs from database");
        var configs = await _context.ServiceChargeConfigs
            .Select(s => new ServiceChargeConfigDto
            {
                ServiceChargeConfigId = s.serviceChargeConfigId,
                RegistrationNumber = s.registrationNumber,
                Percentage = s.percentage,
                ValidFrom = s.validFrom,
                ValidTo = s.validTo
            })
            .ToListAsync();
        
        _logger.LogInformation("Fetched {Count} service charge configs", configs.Count);
        return configs;
    }

    public async Task<ServiceChargeConfigDto> CreateAsync(ServiceChargeConfigCreateDto dto)
    {
        _logger.LogInformation("Creating new service charge config: {@Dto}", dto);

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

        _logger.LogInformation("Created service charge config with ID {ConfigId}", entity.serviceChargeConfigId);

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
