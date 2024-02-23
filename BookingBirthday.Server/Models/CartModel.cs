using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public string? Package_Name { get; set; }

        //GuestId
        public int BookingId { get; set; }
        public int PackageId { get; set; }
        public int ServiceId { get; set; }
        public Double? Price { get; set; }
        public Package? Package { get; set; }
        public Service? Service { get; set; }
    }
}
