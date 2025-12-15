using System;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.dto
{
    public class ProductStaffDto
    {
        public Guid ProductStaffId { get; set; }
        public Guid ProductId { get; set; }
        public int StaffId { get; set; }
        public bool Status { get; set; }
        public DateTime? ValideFrom { get; set; }
        public DateTime? ValideTo { get; set; }
    }
}
