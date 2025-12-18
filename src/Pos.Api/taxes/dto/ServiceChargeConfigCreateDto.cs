using System.ComponentModel.DataAnnotations;
namespace Pos.Api.taxes.dto;

public class ServiceChargeConfigCreateDto
{
    [Required]
    public string RegistrationNumber { get; set; }

    [Required]
    public decimal Percentage { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
}
