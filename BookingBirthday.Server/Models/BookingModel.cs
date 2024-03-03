using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Models
{
    public class BookingModel
    {   
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_start { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public string? BookingStatus { get; set; }
        public double Total { get; set; }
        //GuestId
        public int UserId { get; set; }
        //PaymentId
        public int PaymentId { get; set; }
        public List<CartModel>? CartModels { get; set; }
    }
}
