namespace BookingTime.DTO.RequestModel
{
    public class AddAirportTaxiRequestModel
    {
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string companyName { get; set; }
        public string operatingAirport { get; set; }
        public int bookingPerDay { get; set; }
        public int fleetSize { get; set; }
        public string website { get; set; }
        public string vehicleType { get; set; }
        public int capacity { get; set; }
        public decimal basePrice { get; set; }
        public int currency { get; set; }
        public string availabilityStatus { get; set; }
        public string description { get; set; }
        public IFormFile image { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string status { get; set; }
    }
}