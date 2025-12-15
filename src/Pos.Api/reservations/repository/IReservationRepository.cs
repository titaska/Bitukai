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
        Task<bool> EmployeeIsBusy(int employeeId, DateTime start, int durationMinutes);

        Task<List<DateTime>> GetTakenSlotsAsync(int employeeId, DateTime date);

    }
}
