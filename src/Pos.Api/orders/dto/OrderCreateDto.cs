namespace Pos.Api.orders.dto;

public class OrderCreateDto
{
    public string registrationNumber { get; set; }
    public Guid? customerId { get; set; }
}
