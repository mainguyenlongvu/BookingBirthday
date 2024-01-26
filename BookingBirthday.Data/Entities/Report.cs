using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Report
    {
        public string Id { get; set; }
        public string Detail { get; set; }

        //HostId
        public string HostId { get; set; }
        public Host Host { get; set; }

        //GuestId
        public string GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
