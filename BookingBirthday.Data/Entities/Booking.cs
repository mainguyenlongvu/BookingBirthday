using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date_order { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_cancel { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public string? Reason { get; set; }

        public String? BookingStatus { get; set; }
        public double Total { get; set; }

        //User
        public int UserId { get; set; }
        public User? User { get; set; }
        //Location
        public int LocationId { get; set; }
        public Location Location { get; set; }
        //Package
        public int PackageId { get; set; }
        public Package? Package { get; set; }

        //DepositPaymentId
        public int? DepositPaymentId { get; set; }
        public DepositPayment? DepositPayments { get; set; }

        //RemainingPaymentId
        public int? RemainingPaymentId { get; set; }
        public RemainingPayment? RemainingPayments { get; set; }

    }
}
