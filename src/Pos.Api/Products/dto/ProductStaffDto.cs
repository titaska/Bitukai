namespace Pos.Api.Products.dto;

public class ProductStaffDto
{
    
    public Guid productStaffId { get; set; }
    public Guid productId { get; set; }
    public Guid staffId { get; set; }
    public bool status { get; set; }
    public DateTime? validFrom { get; set; }
    public DateTime? validTo { get; set; }

}
