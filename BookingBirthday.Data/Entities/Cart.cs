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
        public double? Price { get; set; } // Nullable Price property
        public string PackageName { get; set; } // Renamed Package_Name to follow naming convention

        // Foreign key properties
        public int BookingId { get; set; }
        public int PackageId { get; set; }
        public int ServiceId { get; set; }

        // Navigation properties
        public Booking Booking { get; set; }
        public Package Package { get; set; }
        public Service Service { get; set; }

        // Collection navigation properties
        public ICollection<CartService> CartServices { get; set; } = new List<CartService>(); // Initialize collection in constructor
        public ICollection<CartPackage> CartPackages { get; set; } = new List<CartPackage>(); // Renamed to follow naming convention
    }
}
