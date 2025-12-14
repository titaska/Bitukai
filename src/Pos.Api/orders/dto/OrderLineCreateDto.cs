namespace Pos.Api.orders.dto;

public class OrderLineCreateDto
{
    public Guid productId { get; set; }
    public int quantity { get; set; }
    public string? assignedStaffId { get; set; }
    public string? appointmentId { get; set; }
    public string? notes { get; set; }
    //public List<OrderLineOptionDto> options { get; set; }
}
