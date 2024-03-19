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
            modelBuilder.ApplyConfiguration(new BookingPackageConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartPackageConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new DepositPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new RemainingPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());

            // Generate data
            SeedData.Initialize(modelBuilder);
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPackage> BookingPackages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CartPackage> CartPackages { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<DepositPayment> DepositPayments { get; set; }
        public DbSet<RemainingPayment> RemainingPayments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category_requests> Category_Requests { get; set; }
    }
}
