using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int AreaId { get; set; }
        public string Status { get; set; }
        public Area Area { get; set; }
    }
}
