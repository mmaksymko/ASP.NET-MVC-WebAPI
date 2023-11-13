using System.ComponentModel.DataAnnotations;

namespace LiBaby.ViewModels;

public class ReaderViewModel
{
	[Required]
	public int ReaderId { get; set; }
	[Required]
	public string FirstName { get; set; } = null!;
	[Required]
	public string LastName { get; set; } = null!;
	[Required]
	public DateOnly Birthday { get; set; }
	[Required]
	public string Email { get; set; } = null!;
	[Required]
	public string Address { get; set; } = null!;
	[Required]
	public DateTime RegistrationDate { get; set; }
}