using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class OdometerReading
{
    public int Id { get; set; }

    public string? OdometerReading1 { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
