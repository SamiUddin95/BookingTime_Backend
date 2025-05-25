using System.ComponentModel.DataAnnotations;

namespace BookingTime.DTO.RequestModel
{
    public class UserModel
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public int groupId { get; set; }
        public int userId { get; set; }
        public bool isVerified { get; set; }
    }
}
