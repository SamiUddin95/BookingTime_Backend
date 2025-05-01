using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyBeachAccess
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? BeachAccessId { get; set; }
}
