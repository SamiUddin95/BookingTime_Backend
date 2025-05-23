using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class AirportTaxi
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;
    public string OperatingAirport { get; set; } = null!;
    public int CountryId { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public int BookingPerDay { get; set; }

    public int FleetSize { get; set; }

    public string? Website { get; set; }

    public string VehicleType { get; set; } = null!;

    public int Capacity { get; set; }

    public decimal BasePrice { get; set; }

    public int Currency { get; set; }

    public string AvailabilityStatus { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
