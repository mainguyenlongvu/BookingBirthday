using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Cart
    {
        public string Id { get; set; }
        public double total { get; set; }
        public string GuestId { get; set; }
        public Guest Guest { get; set;}
        //GuestId
    }
}
