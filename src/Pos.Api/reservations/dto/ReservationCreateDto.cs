namespace Pos.Api.reservations.dto
{
    public class ReservationCreateDto
    {
        public string RegistrationNumber { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string ServiceProductId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
}
