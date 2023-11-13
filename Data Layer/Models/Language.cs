using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string NativeName { get; set; } = null!;
}
