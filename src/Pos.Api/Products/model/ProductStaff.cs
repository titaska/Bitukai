namespace Pos.Api.Products.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ProductStaff")]
public class ProductStaff
{
    [Key]
    [Column("productStaffId")]
    public Guid productStaffId { get; set; }
    
    //FK to product
    [Column("productId")]
    public Guid productId { get; set; }
    
    //Fk to staff
    [Column("staffId")]
    public int staffId { get; set; }
    
    [Column("status")]
    public bool status { get; set; }
    
    [Column("valideFrom")]
    public DateTime? valideFrom { get; set; }
    
    [Column("valideTo")]
    public DateTime? valideTo { get; set; }
    
}
