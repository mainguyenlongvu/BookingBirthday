using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Package> Packages { get; set; }
    }
}
