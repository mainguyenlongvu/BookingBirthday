using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Payment
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Type Type { get; set; }


        //BookingId
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
