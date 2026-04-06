namespace HealthCareSmart.Core.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string PrescriptionInstructions { get; set; } = string.Empty;
        public string PrescriptionMedicines { get; set; } = string.Empty;
    }

    public class CreateAppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}