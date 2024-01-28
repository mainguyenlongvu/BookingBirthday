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
        public int Id { get; set; }
        public string   Username      { get; set; }
        public string   Password      { get; set; }
        public string   Name          { get; set; }
        public Gender   Gender        { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string   Email         { get; set; }
        public string   Address       { get; set; }
        public string   Phone         { get; set; }

        //CartId
        public int CartId { get; private set; }
        public Cart Cart { get; private set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
