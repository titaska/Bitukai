using Pos.Api.taxes.dto;
using Pos.Api.taxes.model;
using Pos.Api.taxes.repository;
using Microsoft.Extensions.Logging;

namespace Pos.Api.taxes.service
{
    public class TaxService
    {
        private readonly ITaxRepository _repo;
        private readonly ILogger<TaxService> _logger;
        
        public TaxService(ITaxRepository repo, ILogger<TaxService> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        public async Task<List<TaxDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all taxes from repository");
            var list = await _repo.GetAllAsync();
            var count = list.Count;
            _logger.LogInformation("Fetched {Count} taxes", count);
            return list.Select(MapToDto).ToList();
        }
        
        public async Task<TaxDto?> GetByIdAsync(string id)
        {
            _logger.LogInformation("Fetching tax with ID {TaxId}", id);
            var tax = await _repo.GetByIdAsync(id);
            if (tax == null)
            {
                _logger.LogWarning("Tax with ID {TaxId} not found", id);
                return null;
            }
            _logger.LogInformation("Tax with ID {TaxId} found", id);
            return MapToDto(tax);
        }
        
        public async Task<TaxDto> CreateAsync(TaxCreateDto dto)
        {
            _logger.LogInformation("Creating new tax: {@Dto}", dto);
            var tax = new Tax
            {
                name = dto.name,
                description = dto.description,
                percentage = dto.percentage
            };
            
            await _repo.CreateAsync(tax);
            _logger.LogInformation("Created tax with ID {TaxId}", tax.id);
            return MapToDto(tax);
        }

        public async Task UpdateAsync(string id, TaxCreateDto dto)
        {
            _logger.LogInformation("Updating tax with ID {TaxId}: {@Dto}", id, dto);
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception($"Tax with ID {id} not found");

            tax.name = dto.name;
            tax.description = dto.description;
            tax.percentage = dto.percentage;

            await _repo.UpdateAsync(tax);
            _logger.LogInformation("Updated tax with ID {TaxId}", id);
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogInformation("Deleting tax with ID {TaxId}", id);
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception($"Tax with ID {id} not found");
            
            await _repo.DeleteAsync(tax);
            _logger.LogInformation("Deleted tax with ID {TaxId}", id);
        }
        
        private TaxDto MapToDto(Tax tax) =>
            new TaxDto
            {
                id = tax.id,
                name = tax.name,
                description = tax.description,
                percentage = tax.percentage
            };
    }
}
