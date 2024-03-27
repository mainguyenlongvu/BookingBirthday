using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Age
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Package>? Package { get; set; }
    }
}
