namespace BookingBirthday.Server.Models
{
    public class BookingPackageModel
    {
        public int Booking_Package_Id { get; set; }
        public int Booking_id { get; set; }
        public int Package_Id { get; set; }
        public string? Package_name { get; set; }
        public double price { get; set; }
    }
}
