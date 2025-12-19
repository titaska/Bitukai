using Pos.Api.reservations.dto;
using Pos.Api.reservations.model;
using Pos.Api.reservations.repository;

namespace Pos.Api.reservations.service
{
    public class ReservationService
    {
        private readonly IReservationRepository _repo;

        public ReservationService(IReservationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ReservationDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<List<ReservationWithDetailsDto>> GetAllWithDetailsAsync()
        {
            var list = await _repo.GetAllWithDetailsAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<ReservationDto?> GetByIdAsync(string id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task<ReservationDto> CreateAsync(ReservationCreateDto dto)
        {
            // Prevent overlapping appointments
            bool busy = await _repo.EmployeeIsBusy(
                dto.EmployeeId, dto.StartTime, dto.DurationMinutes);

            if (busy)
                throw new Exception("Employee is already booked for that time.");

            var reservation = new Reservation
            {
                AppointmentId = Guid.NewGuid().ToString(),
                RegistrationNumber = dto.RegistrationNumber,
                ServiceProductId = dto.ServiceProductId,
                EmployeeId = dto.EmployeeId,
                StartTime = dto.StartTime,
                DurationMinutes = dto.DurationMinutes,
                Status = "BOOKED",
                Notes = dto.Notes,

                ClientName = dto.ClientName,
                ClientSurname = dto.ClientSurname,
                ClientPhone = dto.ClientPhone
            };


            await _repo.CreateAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task UpdateStatusAsync(string id, string status)
        {
            var r = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Reservation not found");

            r.Status = status;
            await _repo.UpdateAsync(r);
        }

        public async Task UpdateAsync(string id, ReservationCreateDto dto)
        {
            bool busy = await _repo.EmployeeIsBusy(
                dto.EmployeeId, dto.StartTime, dto.DurationMinutes);

            if (busy)
                throw new Exception("Employee is already booked for that time.");

            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");
            
            reservation.RegistrationNumber = dto.RegistrationNumber;
            reservation.EmployeeId = dto.EmployeeId;
            reservation.ServiceProductId = dto.ServiceProductId;
            reservation.StartTime = dto.StartTime;
            reservation.DurationMinutes = dto.DurationMinutes;
            reservation.ClientName = dto.ClientName;
            reservation.ClientSurname = dto.ClientSurname;
            reservation.ClientPhone = dto.ClientPhone;
            reservation.Notes = dto.Notes;
            
            await _repo.UpdateAsync(reservation);
        }

        public async Task DeleteAsync(string id)
        {
            var r = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Reservation not found");

            await _repo.DeleteAsync(r);
        }

        public async Task<List<DateTime>> GetTakenSlotsAsync(Guid employeeId, DateTime date)
        {
            return await _repo.GetTakenSlotsAsync(employeeId, date.Date);
        }


        private ReservationDto MapToDto(Reservation r) =>
        new ReservationDto
        {
        AppointmentId = r.AppointmentId,
        RegistrationNumber = r.RegistrationNumber,
        ServiceProductId = r.ServiceProductId,
        EmployeeId = r.EmployeeId,
        StartTime = r.StartTime,
        DurationMinutes = r.DurationMinutes,
        Status = r.Status,
        OrderId = r.OrderId,
        Notes = r.Notes,

        ClientName = r.ClientName,
        ClientSurname = r.ClientSurname,
        ClientPhone = r.ClientPhone
        };

        private ReservationWithDetailsDto MapToDto(ReservationWithDetails r) =>
            new ReservationWithDetailsDto
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
                ProductName = r.ProductName,
                StaffName = r.StaffFirstName,
                StaffSurname = r.StaffLastName
            };

    }
}
