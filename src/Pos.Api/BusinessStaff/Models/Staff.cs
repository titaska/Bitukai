using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pos.Api.Products.model;

namespace Pos.Api.BusinessStaff.Models
{
    [Table("Staff")]
    public class Staff
    {
        // PK
        [Key]
        [Column("StaffId")]
        public int staffId { get; set; }

        // FK -> Business (Business.RegistrationNumber)
        [Required]
        [Column("RegistrationNumber")]
        public string registrationNumber { get; set; } = null!;

        // enum -> string
        [Required]
        [Column("Status")]
        public StaffStatus status { get; set; }

        [Required]
        [Column("FirstName")]
        public string firstName { get; set; } = null!;

        [Required]
        [Column("LastName")]
        public string lastName { get; set; } = null!;

        [Required]
        [Column("Email")]
        public string email { get; set; } = null!;

        [Required]
        [Column("PhoneNumber")]
        public string phoneNumber { get; set; } = null!;

        [Required]
        [Column("Role")]
        [MaxLength(50)]
        public string role { get; set; } = null!;

        [Required]
        [Column("HireDate")]
        public DateTime hireDate { get; set; }

        [Required]
        [Column("Password")]
        public string Password { get; set; } = null!;

        [ForeignKey(nameof(registrationNumber))]
        public Business Business { get; set; } = null!;

        public ICollection<ProductStaff> productAssignments { get; set; }
            = new List<ProductStaff>();
    }
}
