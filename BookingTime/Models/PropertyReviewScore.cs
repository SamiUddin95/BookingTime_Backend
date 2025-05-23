using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyReviewScore
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? ReviewScoreId { get; set; }
}
