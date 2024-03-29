using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class ThemeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
    }
}
