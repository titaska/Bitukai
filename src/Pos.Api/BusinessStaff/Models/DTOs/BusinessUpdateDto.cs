using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class BusinessUpdateDto
    {
        [Required]
        public string VatCode { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        [Required]
        [MaxLength(3)]
        public string CurrencyCode { get; set; } = null!;
    }
}
