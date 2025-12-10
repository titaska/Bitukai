using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.taxes.model;
using Pos.Api.Context;

namespace Pos.Api.taxes.repository
{
    public class TaxRepository : ITaxRepository
    {
        private readonly AppDbContext _db;

        public TaxRepository(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task<List<Tax>> GetAllAsync() =>
            await _db.Taxes.ToListAsync();
        
        public async Task<Tax?> GetByIdAsync(string id) =>
            await _db.Taxes.FirstOrDefaultAsync(t => t.id == id);
        
        public async Task CreateAsync(Tax tax)
        {
            _db.Taxes.Add(tax);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tax tax)
        {
            _db.Taxes.Update(tax);
            await _db.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(Tax tax)
        {
            _db.Taxes.Remove(tax);
            await _db.SaveChangesAsync();
        }
    }
}
