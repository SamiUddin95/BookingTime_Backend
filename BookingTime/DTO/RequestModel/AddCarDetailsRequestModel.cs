using Microsoft.AspNetCore.Mvc;

namespace BookingTime.DTO.RequestModel
{
    public class AddCarDetailsRequestModel
    {
        public int countryId { get; set; }
        public int cityId { get; set; }
        public int stateId { get; set; }
        public string street { get; set; }
        public long vin { get; set; }
        public int yearId { get; set; }
        public int makeId { get; set; }
        public string? model { get; set; }
        public int odometerId { get; set; }
        public string? vehicleValue { get; set; }
        public int vehicleConditionId { get; set; }
        public bool seatbelts { get; set; }
        public int? seatbeltTypeId { get; set; }
        public string mobileNumber1 { get; set; }
        public string? mobileNumber2 { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int mileageLimit { get; set; }
        public int fuelTypeId { get; set; }
        public string? features { get; set; }
        public string transmission { get; set; }
        public string additionalInfo { get; set; }
        public IFormFile? image { get; set; }
        public List<IFormFile>? carImages { get; set; }
    }
/*    public class CarImageModel
    {
        public IFormFile image { get; set; }
    }*/
}
