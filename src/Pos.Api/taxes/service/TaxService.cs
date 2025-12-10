using Pos.Api.taxes.dto;
using Pos.Api.taxes.model;
using Pos.Api.taxes.repository;

namespace Pos.Api.taxes.service
{
    public class TaxService
    {
        private readonly ITaxRepository _repo;
        
        public TaxService(ITaxRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<List<TaxDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(MapToDto).ToList();
        }
        
        public async Task<TaxDto?> GetByIdAsync(string id)
        {
            var tax = await _repo.GetByIdAsync(id);
            return tax == null ? null : MapToDto(tax);
        }
        
        public async Task<TaxDto> CreateAsync(TaxCreateDto dto)
        {
            var tax = new Tax
            {
                name = dto.name,
                description = dto.description,
                percentage = dto.percentage
            };
            
            await _repo.CreateAsync(tax);
            return MapToDto(tax);
        }

        public async Task UpdateAsync(string id, TaxCreateDto dto)
        {
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Tax not found");

            tax.name = dto.name;
            tax.description = dto.description;
            tax.percentage = dto.percentage;

            await _repo.UpdateAsync(tax);
        }

        public async Task DeleteAsync(string id)
        {
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Tax not found");
            
            await _repo.DeleteAsync(tax);
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
