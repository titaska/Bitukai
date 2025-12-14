using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pos.Api.BusinessStaff.Models
{
    public class Business
    {
        // PK
        public string RegistrationNumber { get; set; } = null!;

        public string VatCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? Phone { get; set; }      // optional
        public string? Email { get; set; }      // optional
        public string CurrencyCode { get; set; } = null!;
        
        [Required]
        public BusinessType Type { get; set; }
        

        // Navigation
        public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    }
}
