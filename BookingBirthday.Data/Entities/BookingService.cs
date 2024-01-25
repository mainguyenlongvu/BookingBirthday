using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class BookingService
    {
        public int ServiceId { get; set; }
        public string PackageId { get; set; }
        public Service Service { get; set; }
        public Package Package { get; set; }
        //PackageId
        //ServiceId
    }
}
