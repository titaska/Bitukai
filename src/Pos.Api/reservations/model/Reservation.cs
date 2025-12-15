using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.reservations.model;

    [Table("reservations", Schema = "point_of_sale")]
    public class Reservation
    {
        [Key]
        [Column("appointmentId")]
        public string AppointmentId { get; set; } = Guid.NewGuid().ToString();

        [Column("registrationNumber")]
        public string RegistrationNumber { get; set; } = null!; // FK → Business

        public string ClientName { get; set; } = null!;
        public string ClientSurname { get; set; } = null!;
        public string ClientPhone { get; set; } = null!;


        [Column("serviceProductId")]
        public string ServiceProductId { get; set; } = null!; // FK → Product (SERVICE)

        [Column("employeeId")]
        
        public int EmployeeId { get; set; }

        [Column("startTime")]
        public DateTime StartTime { get; set; }

        [Column("durationMinutes")]
        public int DurationMinutes { get; set; }

        [Column("status")]
        public string Status { get; set; } = "BOOKED";  // BOOKED|CANCELLED|COMPLETED|NO_SHOW

        [Column("orderId")]
        public string? OrderId { get; set; } // Nullable; set when checkout creates an order

        [Column("notes")]
        public string? Notes { get; set; }
    }

