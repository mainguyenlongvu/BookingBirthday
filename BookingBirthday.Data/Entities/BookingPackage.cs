using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class BookingPackage
    {
        //BookingId
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        //PackageId
        public int PackageId { get; set; }
        public Package Package { get; set; }
        

    }
}
