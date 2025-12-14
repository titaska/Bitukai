using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.BusinessStaff.Models.DTOs;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ServicesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.Services
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new { id = x.Id, name = x.Name, price = x.Price })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Services
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new { id = x.Id, name = x.Name, price = x.Price })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateUpdateDto dto)
        {

            var entity = new ServiceConfig
            {
                Name = dto.Name.Trim(),
                Price = dto.Price
            };

            _context.Services.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new { id = entity.Id, name = entity.Name, price = entity.Price });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceCreateUpdateDto dto)
        {
            var entity = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name.Trim();
            entity.Price = dto.Price;

            await _context.SaveChangesAsync();
            return Ok(new { id = entity.Id, name = entity.Name, price = entity.Price });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null) return NotFound();

            var assignments = await _context.StaffServiceAssignments
                .Where(a => a.ServiceId == id)
                .ToListAsync();

            if (assignments.Count > 0)
                _context.StaffServiceAssignments.RemoveRange(assignments);

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
