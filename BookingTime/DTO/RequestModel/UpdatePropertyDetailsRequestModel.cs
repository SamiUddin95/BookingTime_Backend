namespace BookingTime.DTO.RequestModel
{
    public class UpdatePropertyDetailsRequestModel
    {
        public int PropertyId { get; set; }
        public int? ListTypeId { get; set; }
        public string? ListName { get; set; }
        public string? UsageType { get; set; }
        public string? ShortDesc { get; set; }
        public int? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? LongDesc { get; set; }
        public string? TotalFloor { get; set; }
        public string? TotalRoom { get; set; }
        public string? RoomArea { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? Discount { get; set; }
        public string? PolicyDesc { get; set; }
        public string? CancellationOption { get; set; }
        public decimal? Charges { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public int? RatingId { get; set; }
        public List<amenity> Amenities { get; set; }
        public List<room> rooms { get; set; }

    }

    /*    public class amenity
        {
            public int? AmenitiesId { get; set; }

        }
        public class room
        {
            public string name { get; set; }
            public decimal price { get; set; }
            public decimal discount { get; set; }
            public int additionalInfoId { get; set; }
            public IFormFile Image { get; set; }

        }*/
}
