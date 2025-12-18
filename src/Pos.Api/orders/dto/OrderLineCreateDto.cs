using System.ComponentModel.DataAnnotations;
namespace Pos.Api.orders.dto;

public class OrderLineCreateDto
{
    [Required]
    public Guid productId { get; set; }
    [Required]
    public int quantity { get; set; }
    public string? assignedStaffId { get; set; }
    public string? appointmentId { get; set; }
    public string? notes { get; set; }
    
    public decimal unitPrice  { get; set; }

}
