namespace HealthCareSmart.Core.DTOs
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public List<string> Specializations { get; set; } = new List<string>();
    }
}