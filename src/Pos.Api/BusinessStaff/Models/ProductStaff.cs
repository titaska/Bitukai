using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pos.Api.BusinessStaff.Models
{
    [Table("ProductStaff")]
    public class ProductStaff
    {
        [Key]
        [Column("productStaffId")]
        public Guid productStaffId { get; set; }

        // FK -> Product
        [Required]
        [Column("productId")]
        public Guid productId { get; set; }

        // FK -> Staff
        [Required]
        [Column("staffId")]
        public int staffId { get; set; }

        [Column("status")]
        public bool status { get; set; } = true;

        [Column("valideFrom")]
        public DateTime? valideFrom { get; set; }

        [Column("valideTo")]
        public DateTime? valideTo { get; set; }

        // navigation
        [JsonIgnore]
        public Product? product { get; set; }

        [JsonIgnore]
        public Staff? staff { get; set; }
    }
}
