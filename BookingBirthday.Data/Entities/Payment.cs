using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public double Amount { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
