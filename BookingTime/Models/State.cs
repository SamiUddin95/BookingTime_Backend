using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class State
{
    public long StateId { get; set; }

    public string StateName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;
}
