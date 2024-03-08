﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class PackageService
    {
        //PackageId
        public int PackageId { get; set; }
        public Package Package { get; set; }

        //ServiceId
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}