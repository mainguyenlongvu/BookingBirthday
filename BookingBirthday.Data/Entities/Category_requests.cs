using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Entities
{
    public class Category_requests
    {
        [Key]
        public int category_request_id { get; set; }
        public string? category_name { get; set; }
        public string? host_name { get; set; }
        public string? mail {  get; set; }
        public string? guest_name { get; set; }
        public string? report { get; set; }
        public int requester_id { get; set; }
        public int is_approved { get; set; }
        public bool is_viewed_by_admin { get; set; }
        public bool is_viewed_by_owner { get; set; }
        public bool is_deleted_by_admin { get; set; }
        public bool is_deleted_by_owner { get; set; }
        public string? rejection_reason { get; set; }
        public DateTime created_at { get; set; }
    }
}
