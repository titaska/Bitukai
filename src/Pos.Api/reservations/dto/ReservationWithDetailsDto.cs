namespace Pos.Api.reservations.dto
{
    public class ReservationWithDetailsDto : ReservationDto
    {
        public string ProductName { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string StaffSurname { get; set; } = null!;
    }
}
