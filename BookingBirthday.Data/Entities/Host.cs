using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    internal class Host
    {
        public int GuestId { get; set; }
        public string Username { get; set; }
        public int Password { get; set; }
        public int Name { get; private set; }
        public int Name { get; private set; }
        public int Gender { get; private set; }
        public int DateOfBirth { get; private set; }
        public int Email { get; private set; }
        public int Address { get; private set; }
        public int Phone { get; private set; }

    }
}
