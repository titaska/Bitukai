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

        public async Task<TaxDto?> GetByIdAsync(Guid id)
        {
            var tax = await _repo.GetByIdAsync(id);
            return tax == null ? null : MapToDto(tax);
        }

        public async Task<TaxDto> CreateAsync(TaxCreateDto dto)
        {
            var tax = new Tax
            {
                Name = dto.Name,
                Description = dto.Description,
                Percentage = dto.Percentage
            };

            await _repo.CreateAsync(tax);
            return MapToDto(tax);
        }

        public async Task UpdateAsync(Guid id, TaxCreateDto dto)
        {
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Tax not found");

            tax.Name = dto.Name;
            tax.Description = dto.Description;
            tax.Percentage = dto.Percentage;

            await _repo.UpdateAsync(tax);
        }

        public async Task DeleteAsync(Guid id)
        {
            var tax = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Tax not found");

            await _repo.DeleteAsync(tax);
        }

        private static TaxDto MapToDto(Tax tax) =>
            new TaxDto
            {
                Id = tax.Id,
                Name = tax.Name,
                Description = tax.Description,
                Percentage = tax.Percentage
            };
    }
}
