using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class UserRateModel
    {
        public User User { get; set; }
        public Rate Rate { get; set; }
    }
}
