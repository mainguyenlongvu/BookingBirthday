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
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
      
        public String? BookingStatus { get; set; }
        public double Total { get; set; }

        //UserId
        public int UserId { get; set; }
        public User? User { get; set; }

        //PaymentId
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }



        public List<Cart>? Cart { get; set; }
        public List<BookingPackage>? BookingPackages { get; set; }
    }
}
