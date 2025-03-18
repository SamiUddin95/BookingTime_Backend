using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class FuelType
{
    public int Id { get; set; }

    public string? FuelType1 { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
