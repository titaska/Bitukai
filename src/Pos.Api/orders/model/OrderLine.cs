using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.Orders.Model;

[Table("OrderLines")]
public class OrderLine
{
    //PK
    [Key]
    [Column("orderLineId")]
    public string orderLineId { get; set; }
    
    //FK
    [Column("orderId")]
    public string orderId { get; set; }
    
    //FK
    [Column("productId")]
    public string productId { get; set; }
    
    [Column("quantity")]
    public int quantity { get; set; }
    
    //FK
    [Column("assignedStaffId")]
    public string? assignedStaffId { get; set; }
    
    //FK
    [Column("appointmentId")]
    public string? appointmentId { get; set; }
    
    [Column("notes")]
    public string? notes { get; set; }
    
    //snapshot from product+variations at time of adding
    [Column("unitPrice")]
    public decimal unitPrice { get; set; }
    
    //= quantity × unitPrice + variation deltas
    [Column("subTotal")]
    public decimal subTotal { get; set; }
}
