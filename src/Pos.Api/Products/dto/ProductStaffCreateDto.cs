namespace Pos.Api.Products.dto;

public class ProductStaffCreateDto
{
    
    public Guid staffId { get; set; }
    public bool status { get; set; }
    public DateTime? validFrom { get; set; }
    public DateTime? validTo { get; set; }
    
}
