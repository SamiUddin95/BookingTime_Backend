using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyReview
{
    public int Id { get; set; }

    public long PropertyId { get; set; }

    public long UserId { get; set; }

    public int RatingId { get; set; }

    public string Review { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
