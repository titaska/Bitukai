namespace Pos.Api.BusinessStaff.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ServiceStaff")]
public class ServiceStaff
{
    [Key]
    [Column("productStaffId")]
    public Guid serviceStaffId { get; set; }
    
    //FK to product
    [Column("productId")]
    public Guid serviceId { get; set; }
    
    //Fk to staff
    [Column("staffId")]
    public int staffId { get; set; }
    
    [Column("status")]
    public bool status { get; set; }
    
    [Column("valideFrom")]
    public DateTime? valideFrom { get; set; }
    
    [Column("valideTo")]
    public DateTime? valideTo { get; set; }

    public Staff? staff { get; set; }
    public Service? service { get; set; }
    
}
