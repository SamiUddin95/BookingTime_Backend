using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CityTaxiBasePrice
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public int CurrencyId { get; set; }
}
