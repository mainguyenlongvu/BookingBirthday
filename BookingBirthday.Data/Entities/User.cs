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
        public string? Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Image_url { get; set; }
        public string?  Role { get; set; }
        public string? Status { get; set; }

        public List<Booking>? Bookings { get; set; }
        public List<Promotion>? Promotions { get; set; }
    }
}
