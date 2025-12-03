using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pos.Api.Orders.Model;

[Table("OrderLineTaxes")]
public class OrderLineTax
{
    //PK
    [Key]
    [Column("orderLineTaxId")]
    public Guid orderLineTaxId { get; set; }
    
    //FK
    [Column("orderLineId")]
    public Guid orderLineId { get; set; }
    
    //FK
    [Column("taxCode")]
    public string taxCode { get; set; }
    
    //snapshot
    [Column ("taxPercentage")]
    public decimal taxPercentage { get; set; }
    
    [Column("taxAmount")]
    public decimal taxAmount { get; set; }
}
