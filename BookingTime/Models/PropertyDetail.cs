﻿using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyDetail
{
    public long Id { get; set; }

    public int? ListTypeId { get; set; }

    public string? ListName { get; set; }

    public string? UsageType { get; set; }

    public string? ShortDesc { get; set; }

    public int? CountryId { get; set; }

    public long? StateId { get; set; }

    public long? CityId { get; set; }

    public string? PostalCode { get; set; }

    public string? Street { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Amenities { get; set; }

    public string? LongDesc { get; set; }

    public string? TotalFloor { get; set; }

    public string? TotalRoom { get; set; }

    public string? RoomArea { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? BasePrice { get; set; }

    public decimal? Discount { get; set; }

    public string? PolicyDesc { get; set; }

    public string? CancellationOption { get; set; }

    public decimal? Charges { get; set; }

    public string? Thumbnail { get; set; }

    public string? Images { get; set; }

    public int? AmenitiesId { get; set; }

    public int? RatingId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; } = new List<BookingDetail>();

    public virtual City? City { get; set; }

    public virtual Country? Country { get; set; }

    public virtual HotelType? ListType { get; set; }

    public virtual Rating? Rating { get; set; }

    public virtual State? State { get; set; }
}
