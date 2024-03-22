using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Star { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public User Users { get; set; }
        public int PackageId { get; set; }
        public Package Packages { get; set; }
    }
}
