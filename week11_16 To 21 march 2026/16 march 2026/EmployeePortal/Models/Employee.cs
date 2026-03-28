using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(21, 65, ErrorMessage = "Age must be between 21 and 65")]
        public int Age { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }
    }
}