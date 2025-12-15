namespace Pos.Api.BusinessStaff.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum ServiceType
{
    ITEM,
    SERVICE
}

[Table("Services")]
public class Service
{
    //PK
    [Key]
    [Column("productId")]
    public Guid serviceId { get; set; }
    
    //FK to business
    [Column("registrationNumber")] 
    public string registrationNumber { get; set; }
    
    [Column("type")]
    public ServiceType type { get; set; }
    
    [Column("name")]
    public string name { get; set; }
    
    [Column("description")]
    public string description { get; set; }
    
    [Column("basePrice")]
    public decimal basePrice { get; set; }
    
    //FK to Tax
    [Column("taxCode")]
    public string taxCode { get; set; }
    
    [Column("status")]
    public bool status { get; set; }
    
    [Column("durationMinutes")]
    public int durationMinutes { get; set; }

    public ICollection<ServiceStaff> EligibleStaff { get; set; } = new List<ServiceStaff>();

    
}
