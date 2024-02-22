using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bill");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Discount).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.HasIndex(x => x.BookingId).IsUnique();
            builder.Property(x => x.phone).IsRequired();
            builder.Property(x => x.email).IsRequired();
            builder.Property(x => x.note).IsRequired();

            // 1:1 relationship with Booking
            builder.HasOne(x => x.Booking)
                .WithOne(x => x.Bill)
                .HasForeignKey<Booking>(x => x.BillId);
        }
    }
}
