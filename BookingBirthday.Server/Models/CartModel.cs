using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public string? Package_Name { get; set; }

        //GuestId
        public long BookingId { get; set; }
        public int PackageId { get; set; }
        public Double? Price { get; set; }
        public Package? Package { get; set; }
    }
}
