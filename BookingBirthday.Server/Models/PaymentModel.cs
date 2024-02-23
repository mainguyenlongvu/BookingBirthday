using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Types Types { get; set; }


        //BookingId
        public int BookingId { get; set; }
    }
}
