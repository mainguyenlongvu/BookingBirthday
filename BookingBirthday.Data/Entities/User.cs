using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? image_url { get; set; }
        public Role Role { get; set; }


        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Promotion> Promotions { get; set; }
    }
}
