namespace Pos.Api.Products.dto;

public class ProductUpdateDto
{
    
    public string name { get; set; }
    public string description { get; set; }
    public decimal basePrice { get; set; }
    public int? durationMinutes { get; set; }
    public string taxCode { get; set; }
    public bool status { get; set; }
    
}
