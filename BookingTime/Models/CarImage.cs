using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarImage
{
    public long Id { get; set; }

    public long CarId { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual CarDetail Car { get; set; } = null!;
}
