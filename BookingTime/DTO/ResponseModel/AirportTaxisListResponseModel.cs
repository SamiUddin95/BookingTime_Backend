namespace BookingTime.DTO.ResponseModel
{
    public class AirportTaxisListResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Capacity { get; set; }   
        public string Suitcase { get; set; }  
        public string CityName { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
