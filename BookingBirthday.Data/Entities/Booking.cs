using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date_order { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? note { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public double Total { get; set; }

        //UserId
        public int UserId { get; set; }
        public User? User { get; set; }

        //PaymentId
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        //BillId
        public int BillId { get; set; }
        public Bill Bill { get; set; }

        public List<Cart>? Cart { get; set; }
        public IList<BookingPackage> BookingPackages { get; set; }
        public IList<BookingService> BookingServices { get; set; }
    }
}
