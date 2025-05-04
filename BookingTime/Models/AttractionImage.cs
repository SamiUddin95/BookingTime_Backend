using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class AttractionImage
{
    public int Id { get; set; }

    public int AttractionId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool? IsPrimary { get; set; }

    public virtual Attraction Attraction { get; set; } = null!;
}
