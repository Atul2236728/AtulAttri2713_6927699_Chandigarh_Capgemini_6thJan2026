namespace HealthCareSmart.Core.Models
{
    public class DoctorSpecialization
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}