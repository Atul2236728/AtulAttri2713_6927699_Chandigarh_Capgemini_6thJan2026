using System.ComponentModel.DataAnnotations;

namespace EventRegistration.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public string ParticipantName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string EventName { get; set; }
    }
}