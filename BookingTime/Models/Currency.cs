using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public virtual ICollection<Country> Countries { get; } = new List<Country>();
}
