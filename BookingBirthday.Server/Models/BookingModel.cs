using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public double Total { get; set; }

        //GuestId
        public int GuestId { get; set; }

        //HostId
        public int HostId { get; set; }

        //PaymentId
        public int PaymentId { get; set; }

        //BillId
        public int BillId { get; set; }
        public List<CartModel>? CartModels { get; set; }
    }
}
