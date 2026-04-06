namespace HealthCareSmart.Core.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public string Instructions { get; set; } = string.Empty;

        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}