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
        public decimal? Price { get; set; }

        //BookingId
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        //PackageId
        public int PackageId { get; set; }
        public Package? Package { get; set; }
        //ServiceId
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public ICollection<CartService> CartServices { get; set; }
        public ICollection<CartPackage> CartPackages { get; set; }
    }
}
