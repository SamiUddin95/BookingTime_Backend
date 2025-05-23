using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyPropertyFilter
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? PopularFilterId { get; set; }
}
