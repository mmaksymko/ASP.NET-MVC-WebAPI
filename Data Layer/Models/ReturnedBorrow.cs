using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class ReturnedBorrow
{
    public int BorrowId { get; set; }

    public DateOnly ReturnDate { get; set; }
}
