using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyRoom
{
    public long Id { get; set; }

    public long PropertyId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public int? AdditionalInfoId { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<RoomImage> RoomImages { get; } = new List<RoomImage>();
}
