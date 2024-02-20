using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.BookingStatus).HasDefaultValue(BookingStatus.Accepted);
            builder.Property(x => x.Total).IsRequired();
            builder.HasIndex(b => b.GuestId);
            builder.HasIndex(b => b.HostId);
            builder.HasIndex(x => x.BillId).IsUnique();
            builder.HasIndex(x => x.PaymentId).IsUnique();

            //// 1:1 relationship with Bill
            //builder.HasOne(x => x.Bill)
            //    .WithOne(x => x.Booking)
            //    .HasForeignKey<Bill>(x => x.BookingId);

            //// 1:1 relationship with Payment
            //builder.HasOne(x => x.Payment)
            //    .WithOne(x => x.Booking)
            //    .HasForeignKey<Payment>(x => x.BookingId);

            // 1:M relationship with Host
            builder.HasOne(x => x.Host)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.HostId);

            // 1:M relationship with Guest
            builder.HasOne(x => x.Guest).
                WithMany(x => x.Bookings).
                HasForeignKey(x => x.GuestId);
        }
    }
}
