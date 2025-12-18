namespace Pos.Api.taxes.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("serviceChargeConfig")]
public class ServiceChargeConfig
{
    [Key]
    [Column("serviceChargeConfigId")]
    public Guid serviceChargeConfigId { get; set; }
    
    [Required]
    [Column("registrationNumber")]
    public string registrationNumber { get; set; }
    
    [Column("percentage")]
    public decimal percentage { get; set; }
    
    [Column("validFrom")]
    public DateTime? validFrom { get; set; }
    
    [Column("validTo")]
    public DateTime? validTo { get; set; }
    
}
