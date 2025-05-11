using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class City
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int CountryId { get; set; }

    public int? StateId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Attraction> Attractions { get; } = new List<Attraction>();

    public virtual ICollection<BookingDetail> BookingDetails { get; } = new List<BookingDetail>();

    public virtual ICollection<CarDetail> CarDetails { get; } = new List<CarDetail>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PropertyDetail> PropertyDetails { get; } = new List<PropertyDetail>();
}
