namespace Pos.Api.taxes.dto
{
    public class TaxCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Percentage { get; set; }
    }
}

