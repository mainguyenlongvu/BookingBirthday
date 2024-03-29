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
        public string Detail { get; set; }
        public string Note { get; set; }
        public string? image_url { get; set; }
        public string? Gender { get; set; }
        public string Age {get; set; }
        public string PackageType { get; set; }
        public string? Status { get; set; }

        // Foreign key properties
        public int UserId { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }
        public List<Booking>Bookings { get; set; }
        public List<PackageLocation> PackageLocations { get; set; }

        // Collection navigation properties
        //public List<Cart>? Carts { get; set; }
        //Category
        public IList<Rate> Rates { get; set; }
    }
}
