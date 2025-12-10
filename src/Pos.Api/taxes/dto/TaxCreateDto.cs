namespace Pos.Api.taxes.dto
{
    public class TaxCreateDto
    {
        public string name { get; set; }
        public string? description { get; set; }
        public decimal percentage { get; set; }
    }
}

