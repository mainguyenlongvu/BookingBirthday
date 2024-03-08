using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Models
{
    public class RegisterModel
    {
        public int User_id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? Image_url { get; set; }
        public IFormFile? file { get; set; }
    }
}
