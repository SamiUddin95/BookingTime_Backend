namespace BookingTime.DTO.RequestModel
{
    public class AddCarBookingDetailRequestModel
    {
        public int carId { get; set; }
        public string pickupAddress { get; set; }
        public string dropOffAddress { get; set; }
        public DateTime pickUpDate { get; set; }
        public TimeSpan pickUpTime { get; set; }
        public DateTime? returnDate{ get; set; }
        public TimeSpan? returnTime{ get; set; }
        public decimal totalAmount  { get; set; }
        public string distance  { get; set; }
        public int luggages  { get; set; }
        public int passengers  { get; set; }
        public int userId  { get; set; }

        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }


}
