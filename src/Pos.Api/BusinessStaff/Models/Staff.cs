using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Api.BusinessStaff.Models
{
    public class Staff
    {
        // PK
        public int StaffId { get; set; }

        // FK -> Business
        public string RegistrationNumber { get; set; } = null!;

        public StaffStatus Status { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
        public StaffRole Role { get; set; }

        public DateTime HireDate { get; set; }

        // Navigation
        public Business Business { get; set; } = null!;
    }
}
