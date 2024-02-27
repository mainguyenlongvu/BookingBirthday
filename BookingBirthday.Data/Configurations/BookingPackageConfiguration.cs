using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class BookingPackageConfiguration : IEntityTypeConfiguration<BookingPackage>
    {
        public void Configure(EntityTypeBuilder<BookingPackage> builder)
        {
            builder.ToTable("BookingPackage");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.BookingId);
            builder.HasIndex(x => x.PackageId);

            builder.Property(x => x.Price);
            // Foreign Keys
            builder.HasOne(bp => bp.Booking)
                .WithMany(b => b.BookingPackages)
                .HasForeignKey(bp => bp.BookingId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(bp => bp.Package)
                .WithMany(p => p.BookingPackages)
                .HasForeignKey(bp => bp.PackageId)
                .OnDelete(DeleteBehavior.NoAction); ;
        }
    }
}
