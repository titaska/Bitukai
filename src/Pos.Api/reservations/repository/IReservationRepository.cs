using Pos.Api.reservations.model;

namespace Pos.Api.reservations.repository
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(string id);
        Task CreateAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task<bool> EmployeeIsBusy(string employeeId, DateTime start, int durationMinutes);

        Task<List<DateTime>> GetTakenSlotsAsync(string employeeId, DateTime date);

    }
}
