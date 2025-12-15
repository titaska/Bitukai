public class ReservationCreateDto
{
    public string RegistrationNumber { get; set; } = null!;
    public int EmployeeId { get; set; } 
    public string ServiceProductId { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public string ClientName { get; set; } = null!;
    public string ClientSurname { get; set; } = null!;
    public string ClientPhone { get; set; } = null!;
    public string? Notes { get; set; }
}

