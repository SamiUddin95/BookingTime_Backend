namespace BookingTime.DTO.RequestModel
{
    public class CreateAttractionDTO
    {
        public long CityId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public int? CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    }
}
