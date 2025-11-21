namespace Pos.Api.orders.dto;

public class OrderLineDto
{
    public Guid orderLineId { get; set; }
    public Guid orderId { get; set; }
    public Guid productId { get; set; }
    public int quantity { get; set; }
    public Guid? assignedStaffId { get; set; }
    public Guid? appointmentId { get; set; }
    public string? notes { get; set; }
    public decimal unitPrice { get; set; }
    public decimal subTotal { get; set; }

    public List<OrderLineOptionDto> options { get; set; } = new List<OrderLineOptionDto>();
    //public List<OrderLineTaxDto> taxes { get; set; }
    //public List<OrderLineDiscountDto> discounts { get; set; }
}
