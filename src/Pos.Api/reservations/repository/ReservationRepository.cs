using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.Context;
using Pos.Api.reservations.model;

namespace Pos.Api.reservations.repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _db;

        public ReservationRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Reservation>> GetAllAsync() =>
            await _db.Reservations.ToListAsync();
        
        public async Task<List<ReservationWithDetails>> GetAllWithDetailsAsync()
        {
            var reservations = await _db.Reservations
                .AsNoTracking()
                .Where(r => r.Status == "BOOKED")
                .ToListAsync();
            var products = await _db.Products.AsNoTracking().ToDictionaryAsync(p => p.productId.ToString(), p => p.name);
            var staff = await _db.Staff.AsNoTracking().ToDictionaryAsync(s => s.staffId, s => (s.firstName,s.lastName));

            return reservations.Select(r => new ReservationWithDetails
            {
                AppointmentId = r.AppointmentId,
                RegistrationNumber = r.RegistrationNumber,
                ClientName = r.ClientName,
                ClientSurname = r.ClientSurname,
                ClientPhone = r.ClientPhone,
                ServiceProductId = r.ServiceProductId,
                EmployeeId = r.EmployeeId,
                StartTime = r.StartTime,
                DurationMinutes = r.DurationMinutes,
                Status = r.Status,
                OrderId = r.OrderId,
                Notes = r.Notes,
                ProductName = products.TryGetValue(r.ServiceProductId, out var pn) ? pn : null,
                StaffFirstName = staff.TryGetValue(r.EmployeeId, out var s1) ? s1.firstName : null,
                StaffLastName = staff.TryGetValue(r.EmployeeId, out var s2) ? s2.lastName : null
            }).ToList();
        }

        public async Task<Reservation?> GetByIdAsync(string id) =>
            await _db.Reservations.FirstOrDefaultAsync(r => r.AppointmentId == id);

        public async Task CreateAsync(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task<List<DateTime>> GetTakenSlotsAsync(Guid employeeId, DateTime date)
        {
            // Force UTC
            var utcDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

            var startOfDay = utcDate;
            var endOfDay = utcDate.AddDays(1);

            return await _db.Reservations
                .Where(r =>
                    r.EmployeeId == employeeId &&
                    r.StartTime >= startOfDay &&
                    r.StartTime < endOfDay &&
                    r.Status != "Cancelled"
                )
                .Select(r => r.StartTime)
                .ToListAsync();
        }



        public async Task<bool> EmployeeIsBusy(Guid employeeId, DateTime start, int durationMinutes)
        {
            var end = start.AddMinutes(durationMinutes);

            return await _db.Reservations.AnyAsync(r =>
                r.EmployeeId == employeeId &&
                (
                    (start >= r.StartTime && start < r.StartTime.AddMinutes(r.DurationMinutes)) ||
                    (end > r.StartTime && end <= r.StartTime.AddMinutes(r.DurationMinutes))
                ) &&
                r.Status == "BOOKED"
            );
        }
    }
}
