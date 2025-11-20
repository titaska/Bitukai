using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.Orders.Model;

[Table("Orders")]
public class Order
{
    // PRIMARY KEY
    [Key]
    [Column("orderId")]
    public string orderId { get; set; }

    // FK → Business.registrationNumber
    [Column("registrationNumber")]
    [Required]
    public string registrationNumber { get; set; }

    // FK → Customer.customerId  (NULLABLE)
    [Column("customerId")]
    public string? customerId { get; set; }

    // OPEN | CLOSED_PAID | CANCELLED | REFUNDED | PARTIALLY_REFUNDED
    [Column("status")]
    [Required]
    public string status { get; set; }

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
    
}
