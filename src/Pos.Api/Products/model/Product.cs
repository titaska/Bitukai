namespace Pos.Api.Products.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum ProductType
{
    ITEM,
    SERVICE
}

[Table("Products")]
public class Product
{
    //PK
    [Key]
    [Column("productId")]
    public Guid productId { get; set; }
    
    //FK to business
    [Column("registrationNumber")] 
    public Guid registrationNumber { get; set; }
    
    [Column("type")]
    public ProductType type { get; set; }
    
    [Column("name")]
    public string name { get; set; }
    
    [Column("description")]
    public string description { get; set; }
    
    [Column("basePrice")]
    public decimal basePrice { get; set; }
    
    //FK to Tax
    [Column("taxCode")]
    public Guid taxCode { get; set; }
    
    [Column("status")]
    public bool status { get; set; }
    
    [Column("durationMinutes")]
    public int? durationMinutes { get; set; }
    
}
