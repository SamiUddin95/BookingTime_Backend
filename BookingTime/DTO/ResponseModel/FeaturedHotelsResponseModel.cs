namespace BookingTime.DTO.ResponseModel
{
    public class FeaturedHotelsResponseModel
    {
        public int ID { get; set; }
        public string ListName { get; set; }
        public string ListTypeDescription { get; set; }
        public string UsageType { get; set; }
        public string ShortDesc { get; set; }
        public string PolicyDesc { get; set; }
        public string TotalFloor { get; set; }
        public string TotalRoom { get; set; }
        public string RoomArea { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Charges { get; set; }
        public decimal Discount { get; set; }
        public int CurrencyId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string Rating { get; set; }
        public string Thumbnail { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public List<amenity1> amenity { get; set; }
    }
    public class amenity1
    {
        public int amenityId { get; set; }
        public string amenityName { get; set; }
    }

    public class CityGroupedHotelsResponse
    {
        public string CityName { get; set; }
        public List<FeaturedHotelsResponseModel> Properties { get; set; }
    }

}