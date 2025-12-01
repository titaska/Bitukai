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

        public async Task<bool> EmployeeIsBusy(string employeeId, DateTime start, int durationMinutes)
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
