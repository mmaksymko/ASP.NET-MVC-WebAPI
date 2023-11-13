using System.ComponentModel.DataAnnotations;

namespace LiBaby.ViewModels
{
	public class PublisherViewModel
	{
		[Required]
		public int PublisherId { get; set; }
		[Required]
		public string Name { get; set; } = null!;
		[Required]
		public string Country { get; set; } = null!;
		[Required]
		public DateTime PublisherAdded { get; set; }
	}
}
