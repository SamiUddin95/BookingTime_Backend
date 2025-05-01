using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyFacility
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? FacilityId { get; set; }
}
