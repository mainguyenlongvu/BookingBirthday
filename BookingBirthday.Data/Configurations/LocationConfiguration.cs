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
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties

            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.Address).IsUnicode().IsRequired();
            builder.Property(x => x.AreaId).IsRequired();

            // 1:M relationship with Area
            builder.HasOne(b => b.Area)
                   .WithMany(u => u.Location)
                   .HasForeignKey(b => b.AreaId)
            .OnDelete(DeleteBehavior.NoAction);
           


        }
    }
}
