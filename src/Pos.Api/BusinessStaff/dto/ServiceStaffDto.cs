using System;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.BusinessStaff.dto
{
    public class ServiceStaffDto
    {
        public Guid ServiceStaffId { get; set; }
        public Guid ServiceId { get; set; }
        public int StaffId { get; set; }
        public bool Status { get; set; }
        public DateTime? ValideFrom { get; set; }
        public DateTime? ValideTo { get; set; }
    }
}
