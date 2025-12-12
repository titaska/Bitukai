namespace Pos.Api.Products.dto;

public class ProductStaffDto
{
    
    public Guid productStaffId { get; set; }
    public Guid productId { get; set; }
    public string staffId { get; set; }
    public bool status { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

}
