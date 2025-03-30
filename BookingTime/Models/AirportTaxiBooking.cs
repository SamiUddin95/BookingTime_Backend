using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class AirportTaxiBooking
{
    public int Id { get; set; }

    public int TaxiId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal Price { get; set; }

    public string PickupLocation { get; set; } = null!;

    public string DropoffLocation { get; set; } = null!;

    public DateTime PickupDate { get; set; }

    public TimeSpan PickupTime { get; set; }

    public string Description { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string Contact { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public long CreatedBy { get; set; }
}
