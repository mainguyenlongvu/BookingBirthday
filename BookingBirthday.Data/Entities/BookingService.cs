using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class BookingService
    {
        //BookingId
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        //ServiceId
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
