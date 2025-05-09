namespace BookingTime.DTO.ResponseModel
{
    public class PropertyDetailsModelResponseModel
    {
        public int Id { get; set; }
        public string ShortDesc { get; set; }
        public string PolicyDesc { get; set; }
        public List<RoomDetailModel> Rooms { get; set; }
        public List<string> Thumbnail { get; set; }
        public List<string> RoomImages { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Facilities { get; set; }
        public List<RoomFacilityModel> RoomFacilities { get; set; }  
        public List<RoomAccessibilityModel> RoomAccessibility { get; set; }
        public List<RoomImageModel> RoomImageList { get; set; }
    }
    public class RoomFacilityModel
    {
        public int RoomId { get; set; }
        public string Facility { get; set; }
    }

    public class RoomAccessibilityModel
    {
        public int RoomId { get; set; }
        public string Accessibility { get; set; }
    }
    public class RoomDetailModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int AdditionalInfoId { get; set; }
        public string Image { get; set; } = string.Empty;
    }

    public class RoomImageModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }


}
