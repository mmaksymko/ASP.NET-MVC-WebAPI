using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Reader
{
    public int ReaderId { get; set; }

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;
}
