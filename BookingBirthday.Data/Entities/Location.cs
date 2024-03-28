using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public List<PackageLocation> PackageLocations { get; set; }
        public List< Booking> Bookings { get; set; }

    }
}
