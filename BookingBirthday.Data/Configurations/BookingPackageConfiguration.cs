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
            builder.HasKey(bp => new { bp.BookingId, bp.PackageId });

            // Foreign Keys
            builder.HasOne(bp => bp.Booking)
                .WithMany(b => b.BookingPackage)
                .HasForeignKey(bp => bp.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
