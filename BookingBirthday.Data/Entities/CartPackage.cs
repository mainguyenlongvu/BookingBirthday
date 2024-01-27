using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class CartPackage
    {
        //CartId
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        //PackageId
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
