namespace BookingTime.DTO.RequestModel
{
    public class PropertyFeaturedImagesRequestModel
    {
        public int propertyId { get; set; }
        public List<IFormFile> images { get; set; }
    }
}
