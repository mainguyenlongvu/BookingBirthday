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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Date).IsRequired();
			builder.Property(x => x.Success).IsRequired();
			builder.Property(x => x.Token).IsRequired();
			builder.Property(x => x.VnPayResponseCode).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasIndex(b => b.BookingId);

            // 1:M relationship with Booking
            builder.HasOne(b => b.Booking)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(b => b.BookingId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
