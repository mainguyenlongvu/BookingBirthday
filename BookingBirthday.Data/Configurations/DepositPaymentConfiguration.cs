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
    public class DepositPaymentConfiguration : IEntityTypeConfiguration<DepositPayment>
    {
        public void Configure(EntityTypeBuilder<DepositPayment> builder)
        {
            builder.ToTable("DepositPayment");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Date).IsRequired();
			builder.Property(x => x.Success).IsRequired();
			builder.Property(x => x.Token).IsRequired();
			builder.Property(x => x.VnPayResponseCode).IsRequired();
			builder.Property(x => x.OrderDescription).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
			builder.HasIndex(x => x.BookingId).IsUnique();

            // 1:1 relationship with Booking
            builder.HasOne(x => x.Booking)
                .WithOne(x => x.DepositPayments)
                .HasForeignKey<Booking>(x => x.DepositPaymentId);
        }
    }
}
