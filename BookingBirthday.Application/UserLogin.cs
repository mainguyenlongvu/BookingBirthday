using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Application
{
    [Serializable]
    public class UserLogin
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
