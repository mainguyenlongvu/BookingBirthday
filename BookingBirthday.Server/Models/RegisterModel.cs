namespace BookingBirthday.Server.Models
{
    public class RegisterModel
    {
        public int user_id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? confirmPassword { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? role { get; set; }
        public string? image_url { get; set; }
        public IFormFile? file { get; set; }
    }
}
