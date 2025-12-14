using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Pos.Api.BusinessStaff.Models;
using System.Text.Json.Serialization;

namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class StaffCreateDto
    {
        [JsonIgnore]
        public string? RegistrationNumber { get; set; } = null!;   // Business FK

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

        // Kol kas paprastai – priimam hash, jei vėliau darysim auth, čia bus plain password.
        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public StaffRole Role { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}
