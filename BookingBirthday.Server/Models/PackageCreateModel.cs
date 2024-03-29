using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class PackageCreateModel
    {
        public List<Area> Areas { get; set; }
        public List<Location> Locations { get; set; }
        public List<Package> Packages { get; set; }
    }
}
