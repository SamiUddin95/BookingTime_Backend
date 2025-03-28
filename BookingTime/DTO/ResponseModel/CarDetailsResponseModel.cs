namespace BookingTime.DTO.ResponseModel
{
    public class CarDetailsResponseModel
    {
        public int id { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string vin { get; set; }
        public string vehicleYear { get; set; }
        public string vehicleMake { get; set; }
        public string capacity { get; set; }
        public decimal basePrice { get; set; }
        public string model { get; set; }
        public string odometerReading { get; set; }
        public string vehicleValue { get; set; }
        public string vehicleCondition { get; set; }
        public bool seatblets { get; set; }
        public string seatbletType { get; set; }
        public string mobileNumber1 { get; set; }
        public string mobileNumber2 { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int mileageLimit { get; set; }
        public string fuelType { get; set; }
        public string features { get; set; }    
        public string thumbnail { get; set; }
        public List<image> images { get; set; }

        public class CarDetailsResponseModeldetails
        {
            public List<CarDetailsResponseModel> cardetails { get; set; }
            public int TotalCount { get; set; }
        }

    }
    public class image
    {
        public string carImages { get; set; }
    }
}
