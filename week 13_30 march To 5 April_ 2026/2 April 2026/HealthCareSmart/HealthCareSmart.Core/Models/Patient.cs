using System.ComponentModel.DataAnnotations;

namespace HealthCareSmart.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}