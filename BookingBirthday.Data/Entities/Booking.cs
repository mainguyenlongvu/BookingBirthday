﻿using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public BookingStatus Status { get; set; }
        public double Total { get; set; }

        //GuestId
        public int GuestId { get; set; }
        public Guest Guest { get; set; }

        //HostId
        public int HostId { get; set; }
        public Host Host { get; set; }

        //PaymentId
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        //BillId
        public int BillId { get; set; }
        public Bill Bill { get; set; }

        public ICollection<BookingPackage> BookingPackages { get; set; }
        public ICollection<BookingService> BookingServices { get; set; }
    }
}
