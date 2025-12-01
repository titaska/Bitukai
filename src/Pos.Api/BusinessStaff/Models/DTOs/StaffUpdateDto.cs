using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class StaffUpdateDto
    {
        [Required]
        public StaffStatus Status { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public string? PasswordHash { get; set; }   // galima nekeisti

        [Required]
        public StaffRole Role { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}
