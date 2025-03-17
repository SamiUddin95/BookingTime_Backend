using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class CarDetail
{
    public long Id { get; set; }

    public int CountyId { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public string Street { get; set; } = null!;

    public long Vin { get; set; }

    public int YearId { get; set; }

    public int MakeId { get; set; }

    public string? Model { get; set; }

    public int OdometerId { get; set; }

    public string? VehicleValue { get; set; }

    public int VehicleConditionId { get; set; }

    public bool Seatbelts { get; set; }

    public int? SeatbeltTypeId { get; set; }

    public string MobileNumber1 { get; set; } = null!;

    public string? MobileNumber2 { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int MileageLimit { get; set; }

    public int FuelTypeId { get; set; }

    public string Features { get; set; } = null!;

    public string Transmission { get; set; } = null!;

    public string AdditionalInfo { get; set; } = null!;

    public string Photos { get; set; } = null!;

    public virtual ICollection<CarImage> CarImages { get; } = new List<CarImage>();

    public virtual FuelType FuelType { get; set; } = null!;

    public virtual VehicleMake Make { get; set; } = null!;

    public virtual OdometerReading Odometer { get; set; } = null!;

    public virtual SeatbeltType? SeatbeltType { get; set; }

    public virtual VehicleCondition VehicleCondition { get; set; } = null!;

    public virtual VehicleYear Year { get; set; } = null!;
}
