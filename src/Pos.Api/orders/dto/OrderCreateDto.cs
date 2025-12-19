using System.ComponentModel.DataAnnotations;
namespace Pos.Api.orders.dto;

public class OrderCreateDto
{
    [Required]
    public string registrationNumber { get; set; }
    public Guid? customerId { get; set; }
}
