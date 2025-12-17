using Microsoft.EntityFrameworkCore;
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
