using System.ComponentModel.DataAnnotations;

namespace LiBaby.ViewModels
{
	public class BookViewModel
	{
		[Required]
		public int BookId { get; set; }
		[Required]
		public int? PublisherId { get; set; }
		[Required]
		public int ReleaseYear { get; set; }
		[Required]
		public int Pages { get; set; }
		[Required]
		public string Title { get; set; } = null!;
		[Required]
		public DateTime BookAdded { get; set; }
	}
}
