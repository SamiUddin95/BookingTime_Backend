using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PopularFilter
{
    public int Id { get; set; }

    public string? PopularFilters { get; set; }
}
