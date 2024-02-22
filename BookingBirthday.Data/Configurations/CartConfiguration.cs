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
            builder.HasIndex(x => x.Booking).IsUnique();
            builder.HasIndex(x => x.Price).IsUnique();
            builder.HasIndex(x => x.PackageId).IsUnique();
            builder.HasIndex(x => x.ServiceId).IsUnique();

            // 1:N relationship with Booking
            builder.HasOne(x => x.Booking)
                .WithMany(x => x.Cart)
                .HasForeignKey(x => x.BookingId);
            // 1:N relationship with Package
            builder.HasOne(x => x.Package)
                .WithMany(x => x.Cart)
                .HasForeignKey(x => x.PackageId);
            // 1:N relationship with Service
            builder.HasOne(x => x.Service)
                .WithMany(x => x.Cart)
                .HasForeignKey(x => x.ServiceId);
            // modelBuilder.Entity<Order_items>()
            //.HasOne(p => p.Order)
            //.WithMany(b => b.order_Items)
            //.HasForeignKey(p => p.order_id);
            // modelBuilder.Entity<Order_items>()
            // .HasOne(p => p.Product)
            // .WithMany(b => b.order_Items)
            // .HasForeignKey(p => p.product_id);
        }
    }
}
