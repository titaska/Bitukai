using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.Orders.Model;

public enum OrderStatus
{
    OPEN,
    CLOSED_PAID,
    CANCELLED,
    REFUNDED,
    PARTIALLY_REFUNDED
}

[Table("Orders")]
public class Order
{
    // PRIMARY KEY
    [Key]
    [Column("orderId")]
    public Guid orderId { get; set; }

    // FK → Business.registrationNumber
    [Column("registrationNumber")]
    [Required]
    public string registrationNumber { get; set; }

    // FK → Customer.customerId  (NULLABLE)
    [Column("customerId")]
    public Guid? customerId { get; set; }
    
    [Column("status")]
    [Required]
    public OrderStatus status { get; set; }

    [Column("createdAt")]
    public DateTime createdAt { get; set; }
    
    [Column("closedAt")]
    public DateTime? closedAt { get; set; }

    // DECIMAL PROPERTIES (snapshots)
    [Column("serviceChargePct")]
    public decimal serviceChargePct { get; set; }

    [Column("tipAmount")]
    public decimal tipAmount { get; set; }

    [Column("subtotalAmount")]
    public decimal subtotalAmount { get; set; }

    [Column("taxAmount")]
    public decimal taxAmount { get; set; }

    [Column("discountAmount")]
    public decimal discountAmount { get; set; }

    [Column("serviceChargeAmount")]
    public decimal serviceChargeAmount { get; set; }

    [Column("totalDue")]
    public decimal totalDue { get; set; }
    
    
    public List<OrderLine> Lines { get; set; } = new(); //navigacijai
}
