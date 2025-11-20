using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pos.Api.Orders.Model;

[Table("OrderLineOptions")]
public class OrderLineOption
{
    //PK
    [Key]
    [Column("orderLineOptionId")]
    public string orderLineOptionId { get; set; }
    
    //FK
    [Column("orderLineId")]
    public string orderLineId { get; set; }
    
    //name (e.g., “Milk”)
    [Column("name")]
    public string name { get; set; }
    
    //value (e.g., “Oat”)
    [Column("value")]
    public string value { get; set; }
    
    [Column("priceDelta")]
    public string priceDelta { get; set; }
}
