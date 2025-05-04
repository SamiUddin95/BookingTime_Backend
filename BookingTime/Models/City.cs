using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class City
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int CountryId { get; set; }

    public int? StateId { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; } = new List<BookingDetail>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PropertyDetail> PropertyDetails { get; } = new List<PropertyDetail>();
}
