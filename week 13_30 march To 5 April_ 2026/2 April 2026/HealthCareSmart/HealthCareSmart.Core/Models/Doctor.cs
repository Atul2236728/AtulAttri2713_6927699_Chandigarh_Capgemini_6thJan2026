using System.ComponentModel.DataAnnotations;

namespace HealthCareSmart.Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public decimal ConsultationFee { get; set; } = 0m;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<DoctorSpecialization> Specializations { get; set; } = new List<DoctorSpecialization>();
    }
}