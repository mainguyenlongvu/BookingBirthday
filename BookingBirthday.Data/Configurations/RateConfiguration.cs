using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.ToTable("Rate");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Content);
            builder.Property(x => x.Star).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.PackageId);

            // 1:M relationship with Package
            builder.HasOne<Package>(x => x.Packages)
                .WithMany(x => x.Rates)
                .HasForeignKey(x => x.PackageId);

            // 1:M relationship with User
            builder.HasOne<User>(x => x.Users)
                .WithMany(x => x.Rates)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
