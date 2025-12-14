using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;
namespace Pos.Api.BusinessStaff.Models
{
    public class ServiceConfig
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        [JsonIgnore]  
        public ICollection<StaffServiceAssignment> StaffAssignments { get; set; } = new List<StaffServiceAssignment>();
    }
}
