using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public double Total { get; set; }

        //GuestId
        public int GuestId { get; set; }
        public Guest Guest { get; set; }

        public ICollection<CartService> CartServices { get; set; }
        public ICollection<CartPackage> CartPackages { get; set; }
    }
}
