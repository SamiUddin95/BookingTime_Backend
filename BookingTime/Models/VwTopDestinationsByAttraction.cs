using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class VwTopDestinationsByAttraction
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public int? AttractionCount { get; set; }
}
