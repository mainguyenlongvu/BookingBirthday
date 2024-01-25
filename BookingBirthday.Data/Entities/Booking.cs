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
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public double Total { get; set; }
        public double GuestId { get; set; }
        public double HostId { get; set; }
        public double PaymentId { get; set; }
        public DateTime BillId { get; set; }
        public Guest Guest { get; set; }
        public Host Host { get; set; }
        public Payment Payment { get; set; }
        public Bill Bill { get; set; }
        //GuestId
        //HostId
        //PaymentId
        //BillId
    }
}
