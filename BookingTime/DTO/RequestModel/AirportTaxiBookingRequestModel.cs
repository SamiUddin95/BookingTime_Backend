namespace BookingTime.DTO.RequestModel
{
    public class AirportTaxiBookingRequestModel
    {
        public int taxiId { get; set; }
        public DateTime bookingDate { get; set; }
        public decimal price { get; set; }
        public string pickupLocation { get; set; } 
        public string dropoffLocation { get; set; } 
        public DateTime pickupDate { get; set; }
        public TimeSpan pickupTime { get; set; }
        public string description { get; set; } 
        public string firstName { get; set; } 
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string contact { get; set; } 
        public int createdBy { get; set; } 

    }
}
