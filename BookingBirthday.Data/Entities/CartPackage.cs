using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class CartPackage
    {
        public int CartId { get; set; }
        public string PackageId { get; set; }
        public Cart Cart { get; set; }
        public Package Package { get; set; }
        //CartId
        //PackageId
    }
}
