using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime PublisherAdded { get; set; }

    public string Country { get; set; } = null!;
}
