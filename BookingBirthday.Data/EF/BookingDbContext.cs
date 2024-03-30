using BookingBirthday.Data.Configurations;
using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingBirthday.Data.EF
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RateConfiguration());

            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new PackageLocationConfiguration());
            modelBuilder.ApplyConfiguration(new ThemeConfiguration());

            // Generate data
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category_requests> Category_Requests { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PackageLocation> PackageLocations { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Rate> Rates { get; set; }


    }
}
