namespace Pos.Api.Products.dto;
using Pos.Api.Products.model;
using System.ComponentModel.DataAnnotations;

public class ProductCreateDto
{
    [Required]
    public string registrationNumber { get; set; }
    [Required]
    public ProductType productType { get; set; }
    [Required]
    public string name { get; set; }
    public  string description { get; set; }
    [Required]
    public decimal basePrice { get; set; }
    public int? durationMinutes { get; set; }
    [Required]
    public string taxCode { get; set; }
    public bool status { get; set; }
    
}
