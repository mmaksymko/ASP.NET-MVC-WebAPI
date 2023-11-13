using System.ComponentModel.DataAnnotations;

namespace LiBaby.ViewModels
{
	public class EmployeeViewModel
	{
		[Required]
		public int EmployeeId { get; set; }
		[Required]
		public string FirstName { get; set; } = null!;
		[Required]
		public string LastName { get; set; } = null!;
		[Required]
		public DateOnly Birthday { get; set; }
		[Required]
		public int Salary { get; set; }
		[Required]
		public DateTime RegistrationDate { get; set; }
	}
}
