using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Borrow
{
    public int BorrowId { get; set; }

    public int InventoryId { get; set; }

    public int EmployeeId { get; set; }

    public int ReaderId { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly DueDate { get; set; }
}
