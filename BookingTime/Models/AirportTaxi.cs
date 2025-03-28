using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class AirportTaxi
{
    public int Id { get; set; }

    public string? OperatingAirport { get; set; }

    public int? CountryId { get; set; }

    public long? CityId { get; set; }

    public int? BookingPerDay { get; set; }

    public string? FleetSize { get; set; }

    public string? Website { get; set; }

    public string VehicleType { get; set; } = null!;

    public int Capacity { get; set; }

    public decimal BasePrice { get; set; }

    public string? Currency { get; set; }

    public string? AvailabilityStatus { get; set; }

    public string? PickupLocation { get; set; }

    public string? DropoffLocation { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public long? ContactNumber { get; set; }

    public DateTime? PickupTime { get; set; }

    public string? Status { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? BookedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual City? City { get; set; }

    public virtual Country? Country { get; set; }
}
