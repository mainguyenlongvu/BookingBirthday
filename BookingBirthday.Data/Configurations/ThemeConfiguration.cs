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
    public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> builder)
        {
            builder.ToTable("Theme");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties

            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            // 1:M relationship with User
            builder.HasOne(b => b.User)
                   .WithMany(u => u.Theme)
                   .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        }

    }
}
