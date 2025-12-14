namespace Pos.Api.BusinessStaff.Models
{
    public class StaffServiceAssignment
    {
        public int Id { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; } = null!;

        public int ServiceId { get; set; }
        public ServiceConfig Service { get; set; } = null!;

        public decimal Revenue { get; set; }
    }
}
