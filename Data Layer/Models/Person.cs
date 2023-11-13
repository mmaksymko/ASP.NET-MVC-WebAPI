using System;
using System.Collections.Generic;

namespace LiBaby.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public DateTime RegistrationDate { get; set; }
}
