using System.Collections.Generic;

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

        // Navigation
        public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    }
}
