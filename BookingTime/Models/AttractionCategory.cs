using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class AttractionCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Attraction> Attractions { get; } = new List<Attraction>();
}
