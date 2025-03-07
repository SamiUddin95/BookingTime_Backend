using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class HotelType
{
    public int ListTypeId { get; set; }

    public string HotelType1 { get; set; } = null!;

    public string? Description { get; set; }
}
