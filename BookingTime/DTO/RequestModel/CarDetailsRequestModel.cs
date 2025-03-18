namespace BookingTime.DTO.RequestModel
{
    public class CarDetailsRequestModel
    {
        public CarDetail Details { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public class CarDetail
        {

            public int? carId { get; set; }
            public int? makeId { get; set; }
            public int? yearId { get; set; }
            public int? conditionId { get; set; }
            public int? fuelTypeId { get; set; }
            public string? model { get; set; }
            public int? mileageLimit { get; set; }
            public string? location { get; set; }

        }
    }
}
