using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guest");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.Gender).HasDefaultValue(Gender.Male);
            builder.Property(x => x.DateOfBirth).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Address).IsUnicode().IsRequired();
            builder.Property(x => x.Phone).IsUnicode().IsRequired();
            builder.HasIndex(x => x.CartId).IsUnique();

            // 1:1 relationship with Cart
            builder.HasOne(x => x.Cart)
                .WithOne(x => x.Guest)
                .HasForeignKey<Cart>(x => x.GuestId);
        }
    }
}
