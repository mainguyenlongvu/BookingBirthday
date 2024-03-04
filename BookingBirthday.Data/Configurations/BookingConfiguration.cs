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
            builder.Property(x => x.Date_start).IsRequired();
            builder.Property(x => x.BookingStatus).HasDefaultValue("Processing");
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Note);
            builder.Property(x => x.PaymentId).IsRequired(false);
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(x => x.PaymentId);

            // 1:M relationship with User
            builder.HasOne(b => b.User)
                   .WithMany(u => u.Bookings)
                   .HasForeignKey(b => b.UserId);
                    
        }
    }
}
