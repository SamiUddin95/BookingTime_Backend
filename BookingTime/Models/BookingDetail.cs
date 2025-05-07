using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class BookingDetail
{
    public long Id { get; set; }

    public long? CityId { get; set; }

    public long? PropertyDetailId { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public virtual City? City { get; set; }

    public virtual PropertyDetail? PropertyDetail { get; set; }
}
