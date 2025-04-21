namespace BookingTime.DTO.ResponseModel
{
    public class PopularAttractionResponseModel
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
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string Reviews { get; set; }
        public string Thumbnail { get; set; }
        public List<amenity1> amenity { get; set; }
    }

    public class CityGroupedPopularAttractionResponse
    {
        public string CityName { get; set; }
        public List<PopularAttractionResponseModel> Properties { get; set; }
    }
}
