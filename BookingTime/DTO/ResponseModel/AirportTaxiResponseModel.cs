namespace BookingTime.DTO.ResponseModel
{


    public class AirportTaxiResponseModel
    {
        public int id { get; set; } 
        public string country { get; set; } 
        public string city { get; set; } 
        public string state { get; set; } 
        public string companyName { get; set; } 
        public string operatingAirport { get; set; } 
        public int bookingPerDay { get; set; } 
        public string fleetSizeName { get; set; } 
        public string website { get; set; } 
        public int capacity { get; set; } 
        public decimal basePrice { get; set; } 
        public string currency { get; set; } 
        public string symbol { get; set; } 
        public string availabilityStatus { get; set; } 
        public string description { get; set; } 
        public string imageUrl { get; set; } 
        public string firstName { get; set; } 
        public string lastName { get; set; } 
        public string contactNumber { get; set; } 
        public string email { get; set; } 
        public string status { get; set; }
        public List<VehicleTypeDto> vehicleTypes { get; set; } 
    }
    public class VehicleTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class taxiResponseModeldetails
    {
        public List<AirportTaxiResponseModel> Taxidetails { get; set; }
        public int TotalCount { get; set; }
    }
}
