using Pos.Api.taxes.model;

namespace Pos.Api.taxes.repository
{
    public interface ITaxRepository
    {
        Task<List<Tax>> GetAllAsync();
        Task<Tax?> GetByIdAsync(Guid id);
        Task CreateAsync(Tax tax);
        Task UpdateAsync(Tax tax);
        Task DeleteAsync(Tax tax);
    }
}
