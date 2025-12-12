namespace Pos.Api.Products.dto;
using Pos.Api.Products.model;

public class ProductCreateDto
{
    
    public string registrationNumber { get; set; }
    public ProductType productType { get; set; }
    public string name { get; set; }
    public  string description { get; set; }
    public decimal basePrice { get; set; }
    public int? durationMinutes { get; set; }
    public string taxCode { get; set; }
    public bool status { get; set; }
    
}
