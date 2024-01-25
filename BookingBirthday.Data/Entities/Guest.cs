using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Guest
    {
        public String Id { get; set; }
        public string Username { get; set; }
        public String Password { get; set; }
        public String Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public String Email { get; private set; }
        public String Address { get; private set;}
        public int Phone { get; private set; }
        public int CartId { get; private set;}
        public Cart Cart { get; private set; }
        //CartId
    }
}
