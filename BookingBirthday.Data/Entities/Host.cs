﻿using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Host
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }

        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
