using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class CartItem
    {
        public Package? Package { get; set; }
        public Service? Service { get; set; }
    }
}
