using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.BusinessStaff.Models
{
    [Table("Businesses")]
    public class Business
    {
        // PK
        [Key]
        [Column("RegistrationNumber")]
        [MaxLength(50)]
        public string RegistrationNumber { get; set; } = null!;

        [Column("VatCode")]
        [Required]
        [MaxLength(50)]
        public string VatCode { get; set; } = null!;

        [Column("Name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Column("Location")]
        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = null!;

        [Column("Phone")]
        [MaxLength(50)]
        public string? Phone { get; set; }

        [Column("Email")]
        [MaxLength(200)]
        public string? Email { get; set; }

        [Column("CurrencyCode")]
        [Required]
        [MaxLength(3)]
        public string CurrencyCode { get; set; } = null!;

        [Column("Type")]
        [Required]
        public BusinessType Type { get; set; }

        // Navigation: BUSINESS employs STAFF
        public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    }
}
