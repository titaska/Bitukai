namespace Pos.Api.taxes.dto;

public class ServiceChargeConfigDto
{
    public Guid ServiceChargeConfigId { get; set; }
    public string RegistrationNumber { get; set; }
    public decimal Percentage { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
