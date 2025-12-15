namespace Pos.Api.taxes.dto
{
    public class TaxCreateDto
    {
        public string name { get; set; } = string.Empty;
        public string? description { get; set; }
        public decimal percentage { get; set; }
    }
}

