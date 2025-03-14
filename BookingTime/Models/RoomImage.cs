using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class RoomImage
{
    public long Id { get; set; }

    public long RoomId { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual PropertyRoom Room { get; set; } = null!;
}
