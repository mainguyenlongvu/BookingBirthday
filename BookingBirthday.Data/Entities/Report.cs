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
        public string HostId { get; set; }
        public string GuestId { get; set; }
        public Host Host { get; set; }
        public Guest Guest { get; set; }
        //HostId
        //GuestId
    }
}
