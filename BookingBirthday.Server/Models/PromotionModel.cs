using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Models
{
    public class PromotionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double DiscountPercent { get; set; }
        public Status Status { get; set; }

        //HostId
        public int HostId { get; set; }
    }
}
