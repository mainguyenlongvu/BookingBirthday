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
        public double? Price { get; set; }
        public string PackageName { get; set; }

        // Foreign key properties
        public int BookingId { get; set; }
        public int PackageId { get; set; }

        // Navigation properties
        public Booking? Booking { get; set; }
        public Package? Package { get; set; }
    }
}

