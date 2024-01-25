using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Package
    {
        public string Id { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public string Venue { get; set; }

        public string Detail { get; set; }
        public string PromotionId { get; set; }
        public Promotion Promotion { get; set;}
        //PromotionId

    }
}
