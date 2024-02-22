using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Detail { get; set; }
        public string? image_url { get; set; }

        public Package Package { get; set; }
        public List<Cart>? Cart { get; set; }
        public ICollection<CartService> CartServices { get; set; }
        public ICollection<PackageService> PackageServices { get; set; }
        public ICollection<BookingService> BookingServices { get; set; }
    }
}
