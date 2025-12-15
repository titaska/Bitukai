using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.dto
{
    public class StaffCreateDto
    {
        public string RegistrationNumber { get; set; } = null!;
        public StaffStatus Status { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public string Password { get; set; } = null!;
    }
}
