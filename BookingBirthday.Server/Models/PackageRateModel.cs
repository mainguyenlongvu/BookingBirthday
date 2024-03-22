using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class PackageRateModel
    {
        public Package Package { get; set; }
        public Rate Rate { get; set; }
    }
}
