using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class CartService
    {
        public int CartId { get; set; }
        public string ServiceId { get; set; }
        public Cart Cart { get; set; }
        public Service Service { get; set; }
        //CartId
        //ServiceId
    }
}
