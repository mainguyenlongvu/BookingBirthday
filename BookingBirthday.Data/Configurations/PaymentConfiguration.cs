using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingBirthday.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Types).IsRequired();
            builder.HasIndex(x => x.BookingId).IsUnique();

            // 1:1 relationship with Booking
            builder.HasOne(x => x.Booking)
                .WithOne(x => x.Payment)
                .HasForeignKey<Booking>(x => x.PaymentId);
        }
    }
}
