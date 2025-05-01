using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyPropertyRating
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? PropertyRatingId { get; set; }
}
