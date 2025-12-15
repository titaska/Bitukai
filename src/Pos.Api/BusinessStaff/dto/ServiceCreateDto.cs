using Pos.Api.BusinessStaff.Models;
namespace Pos.Api.BusinessStaff.dto
{
    public class ServiceCreateDto
    {
        public Guid ServiceId { get; set; }
        public string RegistrationNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string TaxCode { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public bool Status { get; set; }
    }
}
