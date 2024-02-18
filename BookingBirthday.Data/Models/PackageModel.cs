using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Models
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public int? PromotionId { get; set; }

    }


}
