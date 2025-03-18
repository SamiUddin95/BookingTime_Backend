using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class VehicleMake
{
    public int Id { get; set; }

    public string? VehicleMake1 { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
