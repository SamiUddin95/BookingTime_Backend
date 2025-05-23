using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyEntirePlace
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? EntirePlaceId { get; set; }
}
