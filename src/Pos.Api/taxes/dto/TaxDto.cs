namespace Pos.Api.taxes.dto
{
    public class TaxDto
    {
        public string id { get; set; } = null!;
        public string name { get; set; } = string.Empty;
        public string? description { get; set; }
        public decimal percentage { get; set; }
    }

}

