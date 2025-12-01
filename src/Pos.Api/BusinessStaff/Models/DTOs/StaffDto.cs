using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class StaffDto
    {
        public int StaffId { get; set; }
        public string RegistrationNumber { get; set; } = null!;
        public StaffStatus Status { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public StaffRole Role { get; set; }
        public DateTime HireDate { get; set; }
    }
}
