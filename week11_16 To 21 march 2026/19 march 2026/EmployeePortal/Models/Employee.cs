using System.ComponentModel.DataAnnotations;
namespace EmployeePortal.Models
{
	public class Employee
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Department { get; set; }

		[Range(1, int.MaxValue)]
		public int Salary { get; set; }
	}
}
