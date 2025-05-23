namespace BookingTime.DTO.RequestModel
{
    public class AttractionFilterDTO
    {
        public List<long> CityIds { get; set; } = new();
        public List<int> CategoryIds { get; set; } = new();
    }
}
