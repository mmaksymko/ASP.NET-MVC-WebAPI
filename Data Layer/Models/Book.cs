using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Book
{
    public int BookId { get; set; }

    public int? PublisherId { get; set; }

    public int ReleaseYear { get; set; }

    public int Pages { get; set; }

    public string Title { get; set; } = null!;

    public DateTime BookAdded { get; set; }
}
