namespace Pos.Api.BusinessStaff.dto
{
    public class ServiceUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string TaxCode { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public bool Status { get; set; }
    }
}
