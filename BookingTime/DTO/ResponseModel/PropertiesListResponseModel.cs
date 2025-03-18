namespace BookingTime.DTO.ResponseModel
{
    public class PropertiesListResponseModel
    {
        public int id { get; set; }
        public string listName { get; set; }
        public string listTypeDescription { get; set; }
        public string usageType { get; set; }
        public string shortDesc { get; set; }
        public string policyDesc { get; set; }
        public string totalFloor { get; set; }
        public string totalRoom { get; set; }
        public string roomArea { get; set; }
        public decimal basePrice { get; set; }
        public decimal charges { get; set; }
        public decimal discount { get; set; }
        public int currencyId { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string rating { get; set; }
        public string thumbnail { get; set; }
        public string featuredImages { get; set; }
        public List<amenities> amenities { get; set; }
        public List<propertyRooms> rooms { get; set; }

        public class PropertyResponseModeldetails
        {
            public List<PropertiesListResponseModel> propertydetails { get; set; }
            public int TotalCount { get; set; }
        }
    }
    public class amenities
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class propertyRooms
    {
        public long id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }
        public int additionalInfoId { get; set; }
        public string additionalInfo { get; set; }
        public string thumbnail { get; set; }
    }
}
