namespace BookingTime.DTO.ResponseModel
{
    public class PropertyReviewsResponseModel
    {
        public int reviewId { get; set; }
        public string userName { get; set; }
        public int rating { get; set; }
        public string review { get; set; }
        public DateTime reviewDate { get; set; }

        public class PropertyReviewsResponseModeldetails
        {
            public List<PropertyReviewsResponseModel> reviewdetails { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
