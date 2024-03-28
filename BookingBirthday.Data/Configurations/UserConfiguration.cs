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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.HasIndex(x => x.Name);
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.Gender);
            builder.Property(x => x.DateOfBirth).IsRequired();
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.HasIndex(x => x.Phone).IsUnique();
            builder.HasIndex(x => x.Address); // Define index for Address
            builder.Property(x => x.Address).IsUnicode();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Image_url); // This should not be a primary key
            builder.Property(x => x.ResetPasswordCode);
        }
    }
}
