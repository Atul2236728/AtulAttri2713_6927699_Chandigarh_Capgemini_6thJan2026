namespace HealthCareSmart.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = "Pending";

        public string Notes { get; set; } = string.Empty;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public Prescription? Prescription { get; set; }

        public Billing? Billing { get; set; }
    }
}