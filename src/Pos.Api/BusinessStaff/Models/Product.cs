using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.BusinessStaff.Models
{

    public enum ProductType
    {
        ITEM,
        SERVICE
    }

    [Table("Products")]
    public class Product
    {
        // PK (schema: string)
        [Key]
        [Column("productId")]
        public Guid productId { get; set; }

        // FK -> Business (schema: string)
        [Required]
        [Column("registrationNumber")]
        public string registrationNumber { get; set; } = null!;

        // schema: string
        [Required]
        [Column("type")]
        public ProductType type { get; set; } = ProductType.SERVICE;

        [Required]
        [Column("name")]
        public string name { get; set; } = null!;

        [Column("description")]
        public string? description { get; set; }

        [Column("basePrice", TypeName = "decimal(18,2)")]
        public decimal basePrice { get; set; }

        //FK
        [Required]
        [Column("taxCode")]
        public string taxCode { get; set; } = null!;

        [Column("status")]
        public bool status { get; set; } = true;

        [Column("durationMinutes")]
        public int durationMinutes { get; set; }

        public ICollection<ProductStaff> EligibleStaff { get; set; } = new List<ProductStaff>();
    }
}
