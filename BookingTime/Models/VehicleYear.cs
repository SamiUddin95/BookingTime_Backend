using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class VehicleYear
{
    public int Id { get; set; }

    public int? VehicleYear1 { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
