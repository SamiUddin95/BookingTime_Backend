using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyAmenity
{
    public int Id { get; set; }

    public long PropertyDetailId { get; set; }

    public int AmenityId { get; set; }
}
