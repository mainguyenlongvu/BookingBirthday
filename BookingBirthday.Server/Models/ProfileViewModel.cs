using BookingBirthday.Data.Entities;
using X.PagedList; // Thêm thư viện này

namespace BookingBirthday.Server.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public IPagedList<Package> PagedPackages { get; set; } // Thêm dòng này
    }
}
