using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookingBirthday.Data.EF
{
    public class BookingDBContext : DbContext
    {
        public BookingDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Bill> Bill { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingPackage> BookingPackage { get; set; }
        public DbSet<BookingService> BookingService { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartPackage> CartPackage { get; set; }
        public DbSet<CartService> CartService { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<Host> Host { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<PackageService> PackageService { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Service> Service { get; set; }



    }


}
