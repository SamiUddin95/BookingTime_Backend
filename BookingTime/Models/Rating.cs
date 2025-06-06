﻿using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class Rating
{
    public int Id { get; set; }

    public string? Ratings { get; set; }

    public virtual ICollection<PropertyDetail> PropertyDetails { get; } = new List<PropertyDetail>();
}
