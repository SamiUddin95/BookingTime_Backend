using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class TopDestinationsByAttraction
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int? AttractionCount { get; set; }
}
