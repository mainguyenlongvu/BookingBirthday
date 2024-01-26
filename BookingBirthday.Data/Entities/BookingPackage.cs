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
        public string BookingId { get; set; }
        public Booking Booking { get; set; }

        //PackageId
        public string PackageId { get; set; }
        public Package Package { get; set; }
        

    }
}
