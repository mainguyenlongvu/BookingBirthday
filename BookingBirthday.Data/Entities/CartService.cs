using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class CartService
    {
        //CartId
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        //ServiceId
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
