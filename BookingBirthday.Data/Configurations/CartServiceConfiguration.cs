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
    public class CartServiceConfiguration : IEntityTypeConfiguration<CartService>
    {
        public void Configure(EntityTypeBuilder<CartService> builder)
        {
            builder.ToTable("CartService");

            // Primary Key
            builder.HasKey(cs => new { cs.CartId, cs.ServiceId });

            // Foreign Key
            builder.HasOne(cs => cs.Cart).WithMany(x => x.CartServices).HasForeignKey(cs => cs.CartId);
            builder.HasOne(cs => cs.Service).WithMany(x => x.CartServices).HasForeignKey(cs => cs.ServiceId);
        }
    }
}
