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
        public string? Host_name { get; set; }
        public double Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public string Note { get; set; }
        public string? image_url { get; set; }
        public string? Status { get; set; }

        // Foreign key properties
        public int? PromotionId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Promotion? Promotion { get; set; }
        public User? User { get; set; }

        // Collection navigation properties
        public List<Cart>? Carts { get; set; }
        public IList<CartPackage>? CartPackages { get; set; }
        public IList<BookingPackage>? BookingPackages { get; set; }
    }
}
