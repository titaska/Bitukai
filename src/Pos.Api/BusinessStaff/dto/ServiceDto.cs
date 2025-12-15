using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.dto
{
    public class ServiceDto
    {
        public Guid ServiceId { get; set; }
        public string RegistrationNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string TaxCode { get; set; } = null!;
        public bool Status { get; set; }
        public int DurationMinutes { get; set; }
    }
}
