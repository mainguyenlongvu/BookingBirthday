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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Price); // Assuming Price can be null
            builder.Property(x => x.Package_Name);

            // Indexes
            builder.HasIndex(x => x.BookingId).IsUnique(false); // Foreign key property should not be unique
            builder.HasIndex(x => x.PackageId).IsUnique(false); // Foreign key property should not be unique
            builder.HasIndex(x => x.ServiceId).IsUnique(false); // Foreign key property should not be unique

            // Relationships
            builder.HasOne(x => x.Booking)
                .WithMany(b => b.cart)
                .HasForeignKey(x => x.BookingId);

            builder.HasOne(x => x.Package)
                .WithMany(p => p.Cart)
                .HasForeignKey(x => x.PackageId);

            builder.HasOne(x => x.Service)
                .WithMany(s => s.Cart)
                .HasForeignKey(x => x.ServiceId);
        }
    }
}

