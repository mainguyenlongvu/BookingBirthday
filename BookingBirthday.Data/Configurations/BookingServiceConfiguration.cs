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
    public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
    {
        public void Configure(EntityTypeBuilder<BookingService> builder)
        {
            builder.ToTable("BookingService");

            // Primary Key
            builder.HasKey(bs => new { bs.BookingId, bs.ServiceId });

            // Foreign Key
            builder.HasOne(bs => bs.Booking)
                .WithMany(x => x.BookingServices)
                .HasForeignKey(bs => bs.BookingId);

            builder.HasOne(bs => bs.Service)
                .WithMany(x => x.BookingServices)
                .HasForeignKey(bs => bs.ServiceId);
        }
    }
}
