namespace BookingBirthday.Server.Models
{
    public class BillModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

        //BookingId
        public int BookingId { get; set; }
    }
}
