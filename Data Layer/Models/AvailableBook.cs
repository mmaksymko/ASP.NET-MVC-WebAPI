using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class AvailableBook
{
    public int InventoryId { get; set; }

    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int Pages { get; set; }

    public int ReleaseYear { get; set; }

    public string? BookCondition { get; set; }
}
