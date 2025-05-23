namespace BookingTime.DTO.ResponseModel
{
    public class AttractionDTO
    {
        public int Id { get; set; }
        public long CityId { get; set; }       
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? DetailedDescription { get; set; }
        public string Price { get; set; }
        public double? Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CityName { get; set; }
        public List<string> Images { get; set; } = new List<string>();

    }
}
