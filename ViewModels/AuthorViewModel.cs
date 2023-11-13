using System.ComponentModel.DataAnnotations;

namespace LiBaby.ViewModels
{
	public class AuthorViewModel
	{
		[Required]
		public int AuthorId { get; set; }
		[Required]
		public string FirstName { get; set; } = null!;
		[Required]
		public string LastName { get; set; } = null!;
		[Required]
		public DateOnly Birthday { get; set; }
		[Required]
		public string Bio { get; set; } = null!;
		[Required]
		public DateTime RegistrationDate { get; set; }
	}
}