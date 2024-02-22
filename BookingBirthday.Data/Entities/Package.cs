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
        public decimal Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public string? image_url { get; set; }

        //PromotionId
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        //UserId
        public int UserId { get; set; }
        public User User { get; set; }

        public List<Cart>? Cart { get; set; }
        public IList<CartPackage> CartPackages { get; set; }
        public IList<PackageService> PackageServices { get; set; }
        public IList<BookingPackage> BookingPackages { get; set; }

    }
}
