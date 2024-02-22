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
            builder.Property(x => x.Date_order).IsRequired();
            builder.Property(x => x.BookingStatus).HasDefaultValue(BookingStatus.Accepted);
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Note).IsRequired();
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(x => x.PaymentId).IsUnique();

            //// 1:1 relationship with Bill
            //builder.HasOne(x => x.Bill)
            //    .WithOne(x => x.Booking)
            //    .HasForeignKey<Bill>(x => x.BookingId);

            //// 1:1 relationship with Payment
            //builder.HasOne(x => x.Payment)
            //    .WithOne(x => x.Booking)
            //    .HasForeignKey<Payment>(x => x.BookingId);

            // 1:M relationship with User
            builder.HasOne(x => x.User)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.UserId);
        }
    }
}
