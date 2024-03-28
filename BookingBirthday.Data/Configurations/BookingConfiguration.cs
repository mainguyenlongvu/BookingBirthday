using BookingBirthday.Data.Entities;
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
            builder.Property(x => x.Date_cancel);
            builder.Property(x => x.BookingStatus).HasDefaultValue("Processing");
            builder.Property(x => x.Address).IsRequired().IsUnicode();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Note).IsUnicode();
            builder.Property(x => x.Reason).IsUnicode();
            builder.Property(x => x.ChildName).IsUnicode();
            builder.Property(x => x.ChildDateOfBirth).IsRequired();
            builder.Property(x => x.Gender).IsRequired().IsUnicode();
            builder.Property(x => x.ChildNumber).IsRequired();
           
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(b => b.LocationId);
            builder.HasIndex(b => b.PackageId);

            // 1:M relationship with User
            builder.HasOne(b => b.User)
                   .WithMany(u => u.Bookings)
                   .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);
            // 1:M relationship with Location
            builder.HasOne(x => x.Location)
            .WithMany(b => b.Bookings)
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.NoAction);
            // 1:M relationship with Package
            builder.HasOne(x => x.Package)
            .WithMany(b => b.Bookings)
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
