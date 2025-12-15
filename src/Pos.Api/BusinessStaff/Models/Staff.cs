using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.BusinessStaff.Models
{
    [Table("Staff")]
    public class Staff
    {
        // PK
        [Key]
        [Column("staffId")]
        public int staffId { get; set; }

        // FK -> Business (Business.registrationNumber)
        [Required]
        [Column("registrationNumber")]
        public string registrationNumber { get; set; } = null!;

        // enum -> string (pagal tavo ERD: status yra string)
        [Required]
        [Column("status")]
        public StaffStatus status { get; set; }

        [Required]
        [Column("firstName")]
        public string firstName { get; set; } = null!;

        [Required]
        [Column("lastName")]
        public string lastName { get; set; } = null!;

        [Required]
        [Column("email")]
        public string email { get; set; } = null!;

        [Required]
        [Column("phoneNumber")]
        public string phoneNumber { get; set; } = null!;

        [ForeignKey(nameof(registrationNumber))]
        public Business Business { get; set; } = null!;

        public ICollection<ProductStaff> serviceAssignments { get; set; } = new List<ProductStaff>();
    }
}
