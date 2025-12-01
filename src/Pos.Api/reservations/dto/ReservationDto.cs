namespace Pos.Api.reservations.dto
{
    public class ReservationDto
    {
        public string AppointmentId { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string ServiceProductId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = null!;
        public string? OrderId { get; set; }
        public string? Notes { get; set; }
    }
}
