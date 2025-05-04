namespace BookingTime.DTO.ResponseModel
{
    public class TopDestinationsByAttractionDTO
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public int AttractionCount { get; set; }
    }
}
