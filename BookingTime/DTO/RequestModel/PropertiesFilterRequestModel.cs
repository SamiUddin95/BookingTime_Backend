namespace BookingTime.DTO.RequestModel
{
    public class PropertiesFilterRequestModel
    {
        public Detail Details { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public class Detail
        {

            public int? hotelTypeId { get; set; }
            public int? priceRangeFrom { get; set; }
            public int? priceRangeTo { get; set; }
            public int? ratingId { get; set; }
            public int? amenities { get; set; }
        }
    }
}
