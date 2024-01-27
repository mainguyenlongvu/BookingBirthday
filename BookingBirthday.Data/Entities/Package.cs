using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }

        //PromotionId
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public ICollection<CartPackage> CartPackages { get; set; }
        public ICollection<PackageService> PackageServices { get; set; }
        public ICollection<BookingPackage> BookingPackages { get; set; }

    }
}
