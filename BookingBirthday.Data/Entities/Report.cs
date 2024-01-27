using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Detail { get; set; }

        //HostId
        public int HostId { get; set; }
        public Host Host { get; set; }

        //GuestId
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
