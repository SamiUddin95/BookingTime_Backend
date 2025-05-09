using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public int? CurrencyId { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<PropertyDetail> PropertyDetails { get; } = new List<PropertyDetail>();

    public virtual ICollection<State> States { get; } = new List<State>();
}
