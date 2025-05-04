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
            public int? cityId { get; set; }
            public int? ratingId { get; set; }
            public DateTime? CheckIn { get; set; }
            public DateTime? CheckOut { get; set; }
            public List<hotelType> hotelTypes { get; set; }
            public List<amenity> amenities { get; set; }
            public List<IdWrapper> BeachAccess { get; set; } = new();
            public List<IdWrapper> EntirePlaces { get; set; } = new();
            public List<IdWrapper> Facilities { get; set; } = new();
            public List<IdWrapper> FunThingsToDo { get; set; } = new();
            public List<IdWrapper> PopularFilter { get; set; } = new();
            public List<IdWrapper> PropertyType { get; set; } = new();
            public List<IdWrapper> PropertyAccessibility { get; set; } = new();
            public List<IdWrapper> RoomAccessibility { get; set; } = new();
            public List<IdWrapper> RoomFacilities { get; set; } = new();
        }

        public class hotelType
        {
            public int? hotelTypeId { get; set; }

        }
        public class IdWrapper
        {
            public int Id { get; set; }
        }
    }
}
