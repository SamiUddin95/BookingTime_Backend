namespace BookingTime.DTO.RequestModel
{
    public class AirportTaxiDetailsRequestModel
    {
        public TaxiDetail Detail { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public class TaxiDetail
        {
            public int? taxiId { get; set; }
            public DateTime? pickUpDate { get; set; }
            public TimeSpan? pickUpTime { get; set; }
            public int? cityId { get; set; }
            public decimal? priceFrom { get; set; }
            public decimal? priceTo { get; set; }
        }

    }
}
