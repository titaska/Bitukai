using System.ComponentModel.DataAnnotations;

namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class ServiceCreateUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Range(0, 999999999)]
        public decimal Price { get; set; }
    }
}
