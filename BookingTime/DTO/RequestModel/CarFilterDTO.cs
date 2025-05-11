namespace BookingTime.DTO.RequestModel
{
    public class CarFilterDTO
    {
        public int PickupLocation { get; set; }  
        public int? DropoffLocation { get; set; } 
        public string PickupDate { get; set; }   
        public string PickupTime { get; set; }    
        public string DropoffDate { get; set; }  
        public string DropoffTime { get; set; }    
    }
}
