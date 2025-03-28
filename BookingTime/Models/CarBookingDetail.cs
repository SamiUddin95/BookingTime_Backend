using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarBookingDetail
{
    public long Id { get; set; }

    public int CarId { get; set; }

    public string PickupAddress { get; set; } = null!;

    public string DropoffAddress { get; set; } = null!;

    public DateTime PickupDate { get; set; }

    public TimeSpan PickupTime { get; set; }

    public DateTime? ReturnDate { get; set; }

    public TimeSpan? ReturnTime { get; set; }

    public decimal TotalAmount { get; set; }

    public int Luggages { get; set; }

    public int Passengers { get; set; }

    public string Distance { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public int CreatedBy { get; set; }
}
