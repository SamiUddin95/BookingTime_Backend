using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class VehicleCondition
{
    public int Id { get; set; }

    public string? Condition { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
