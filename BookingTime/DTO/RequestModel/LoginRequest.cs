using System.ComponentModel.DataAnnotations;

namespace BookingTime.DTO.RequestModel
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        public string Password { get; set; }
    }
}
