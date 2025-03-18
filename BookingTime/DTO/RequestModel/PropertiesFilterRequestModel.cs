namespace BookingTime.DTO.RequestModel
{
    public class PropertiesFilterRequestModel
    {
        public Detail Details { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public class Detail
        {

            public int? priceRangeFrom { get; set; }
            public int? priceRangeTo { get; set; }
            public int? ratingId { get; set; }
            public List<hotelType> hotelTypes { get; set; }
            public List<amenity> amenities { get; set; }
        }

        public class hotelType
        {
            public int? hotelTypeId { get; set; }

        }
    }
    
}
