namespace HealthCareSmart.Core.DTOs
{
    public class BillingDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public decimal MedicineCost { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePrescriptionDTO
    {
        public string Instructions { get; set; } = string.Empty;
        public List<MedicineItemDTO> Medicines { get; set; } = new List<MedicineItemDTO>();
    }

    public class MedicineItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
