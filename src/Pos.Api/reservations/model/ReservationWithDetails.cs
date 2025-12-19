namespace Pos.Api.reservations.model;

public class ReservationWithDetails : Reservation
{
    public string ProductName { get; set; } = null!;
    public string StaffFirstName { get; set; } = null!;
    public string StaffLastName { get; set; } = null!;
}
