namespace BookingTime.DTO.RequestModel
{
    public class AddCitytaxiBasePriceRequestModel
    {
        public string cityName{ get; set; } 
        public decimal price{ get; set; } 
        public int currencyId{ get; set; } 
    }
}
