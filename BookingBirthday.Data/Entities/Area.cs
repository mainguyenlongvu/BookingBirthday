﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Location>? Location { get; set; }
    }
}
