using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarBooking
{
    public int BookingId { get; set; }

    public long CarId { get; set; }

    public long UserId { get; set; }

    public string PickupLocation { get; set; } = null!;

    public string DropoffLocation { get; set; } = null!;

    public DateTime PickupDate { get; set; }

    public DateTime DropoffDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string BookingStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CarDetail Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
