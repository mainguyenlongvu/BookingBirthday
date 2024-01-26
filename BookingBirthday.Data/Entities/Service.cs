using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Detail { get; set; }

        public ICollection<CartService> CartServices { get; set; }
        public ICollection<PackageService> PackageServices { get; set; }
        public ICollection<BookingService> BookingServices { get; set; }
    }
}
