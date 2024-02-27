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
                builder.Property(x => x.Price); 

                // Indexes
                builder.HasIndex(x => x.BookingId);
                builder.HasIndex(x => x.PackageId);

            // Relationships
            builder.HasOne(x => x.Booking)
                .WithMany(b => b.Cart)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Package)
                .WithMany(b => b.Carts)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.NoAction);


        }
        }
    }


