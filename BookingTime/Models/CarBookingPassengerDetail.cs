using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarBookingPassengerDetail
{
    public long Id { get; set; }

    public long BookingDetailId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;
}
