namespace Pos.Api.taxes.dto
{
    public class TaxDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public decimal percentage { get; set; }
    }
}

