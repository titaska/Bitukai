namespace Pos.Api.orders.dto;

public class OrderLineUpdateDto
{
    public int? quantity { get; set; }
    public string? assignedStaffId { get; set; }
    public string? notes { get; set; }
}
