﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

        //BookingId
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
