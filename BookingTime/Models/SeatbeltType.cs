using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class SeatbeltType
{
    public int Id { get; set; }

    public string? SeatbeltType1 { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
