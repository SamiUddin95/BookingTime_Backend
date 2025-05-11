using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTime.DTO.ResponseModel
{
    public class AvailableCarsListDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? PassengerCapacity { get; set; }
        public int? Days { get; set; }
        public decimal? TotalRent { get; set; }
        public int? MileageLimit { get; set; }
        public string Transmission { get; set; }
        public string CarImage { get; set; }
        [NotMapped]
        public string? CarImageBase64 { get; set; } = null;
        public string PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }
        public string Currency { get; set; }
    }
}
