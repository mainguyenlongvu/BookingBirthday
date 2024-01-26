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
        public string CartId { get; set; }
        public Cart Cart { get; set; }

        //PackageId
        public string PackageId { get; set; }
        public Package Package { get; set; }
    }
}
