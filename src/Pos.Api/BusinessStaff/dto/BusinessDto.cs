using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Api.BusinessStaff.Models;


namespace Pos.Api.BusinessStaff.dto
{
    public class BusinessDto
    {
        public string RegistrationNumber { get; set; } = null!;
        public string VatCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string CurrencyCode { get; set; } = null!;
        
        public BusinessType Type { get; set; }
    }
}
