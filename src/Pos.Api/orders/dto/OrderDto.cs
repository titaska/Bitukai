using Pos.Api.Orders.Model;

namespace Pos.Api.orders.dto;

public class OrderDto
{
    public Guid orderId { get; set; }
    public string registrationNumber { get; set; }
    public Guid? customerId { get; set; }
    public OrderStatus status { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? closeddAt { get; set; }
    public decimal? serviceChargePct { get; set; }
    public decimal? tipAmount { get; set; }
    public decimal? subtotalAmount { get; set; }
    public decimal? taxAmount { get; set; }
    public decimal? discountAmount { get; set; }
    public decimal? serviceChargeAmount { get; set; }
    public decimal? totalDue { get; set; }

    public List<OrderLineDto> lines { get; set; } = new List<OrderLineDto>();
    //payment list
}
