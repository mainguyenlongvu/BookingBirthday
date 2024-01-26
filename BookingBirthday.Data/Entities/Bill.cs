using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Bill
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

        //BookingId
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
