using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.BusinessStaff.Models.DTOs;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Controllers
{
    [ApiController]
    [Route("api/staff/{staffId:int}/services")]
    public class StaffServicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StaffServicesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get(int staffId)
        {
            var data = await _context.StaffServiceAssignments
                .Where(x => x.StaffId == staffId)
                .Include(x => x.Service)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Service.Name,
                    price = x.Service.Price
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(int staffId, [FromBody] AssignServiceDto dto)
        {
            var exists = await _context.StaffServiceAssignments
                .AnyAsync(x => x.StaffId == staffId && x.ServiceId == dto.ServiceId);

            if (exists) return BadRequest("Already assigned");

            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == dto.ServiceId);
            if (service == null) return NotFound("Service not found");

            var entity = new StaffServiceAssignment
            {
                StaffId = staffId,
                ServiceId = dto.ServiceId,
                Revenue = 0m
            };

            _context.StaffServiceAssignments.Add(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Unassign(int staffId, int id)
        {
            var entity = await _context.StaffServiceAssignments
                .FirstOrDefaultAsync(x => x.Id == id && x.StaffId == staffId);

            if (entity == null) return NotFound();

            _context.StaffServiceAssignments.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
