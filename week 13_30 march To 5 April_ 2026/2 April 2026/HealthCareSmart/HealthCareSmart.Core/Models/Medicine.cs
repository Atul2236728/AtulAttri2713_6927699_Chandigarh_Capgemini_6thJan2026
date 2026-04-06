namespace HealthCareSmart.Core.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0m;

        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; } = null!;
    }
}