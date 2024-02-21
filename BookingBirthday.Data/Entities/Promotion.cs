using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double DiscountPercent { get; set; }
        public Status Status { get; set; }

        //UserId
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Package> Packages { get; set; }
    }
}
