namespace Pos.Api.Products.dto;
using System.ComponentModel.DataAnnotations;

public class ProductStaffCreateDto
{
    [Required]
    public Guid staffId { get; set; }
    public bool status { get; set; }
    public DateTime? validFrom { get; set; }
    public DateTime? validTo { get; set; }
    
}
