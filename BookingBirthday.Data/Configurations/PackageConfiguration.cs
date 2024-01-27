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
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Package");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Venue).IsUnicode().IsRequired();
            builder.Property(x => x.Detail).IsUnicode().IsRequired();

            // 1:M relationship with Promotion
            builder.HasOne<Promotion>(x => x.Promotion)
                .WithMany(x => x.Packages)
                .HasForeignKey(x => x.PromotionId);
        }
    }
}
