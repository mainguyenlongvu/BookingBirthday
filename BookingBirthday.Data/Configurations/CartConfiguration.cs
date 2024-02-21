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

            //// 1:1 relationship with Guest
            //builder.HasOne(x => x.Guest)
            //    .WithOne(x => x.Cart)
            //    .HasForeignKey<Guest>(x => x.CartId);
        }
    }
}
