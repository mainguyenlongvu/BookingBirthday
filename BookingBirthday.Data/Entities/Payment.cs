using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Type Type { get; set; }


        //BookingId
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
