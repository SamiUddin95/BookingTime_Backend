using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyPropertyType
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? PropertyTypeId { get; set; }
}
