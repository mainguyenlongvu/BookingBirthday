using Microsoft.EntityFrameworkCore;

namespace BookingBirthday.Data.EF
{
    public class BookingDBContext : DbContext
    {
        public BookingDBContext(DbContextOptions options) : base(options)
        {

        }

    }


}
