using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class TaxiVechiletype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
