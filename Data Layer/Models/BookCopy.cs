using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class BookCopy
{
    public int InventoryId { get; set; }

    public int BookId { get; set; }

    public string? BookCondition { get; set; }
}
