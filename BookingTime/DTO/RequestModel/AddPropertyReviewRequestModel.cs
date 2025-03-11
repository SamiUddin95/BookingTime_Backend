namespace BookingTime.DTO.RequestModel
{
    public class AddPropertyReviewRequestModel
    {
        public long propertyId { get; set; }
        public long userId { get; set; }
        public int ratingId { get; set; }
        public string review { get; set; }
    }
}
