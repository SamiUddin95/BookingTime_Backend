using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Icon { get; set; }

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();
}
