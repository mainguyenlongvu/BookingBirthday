namespace BookingBirthday.Server.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public double Total { get; set; }

        //GuestId
        public int BookingId { get; set; }
        public int PackageId { get; set; }
        public string? Package_name { get; set; }
        public decimal price { get; set; }
    }
}
