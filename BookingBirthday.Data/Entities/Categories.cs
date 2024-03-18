using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Categories
    {
        public int category_id { get; set; }
        public string? name { get; set; }
        public List<Package>? Package { get; set; }
    }
}
