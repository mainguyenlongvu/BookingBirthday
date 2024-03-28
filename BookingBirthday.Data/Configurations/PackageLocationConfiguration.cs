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
    public class PackageLocationConfiguration : IEntityTypeConfiguration<PackageLocation>
    {
        public void Configure(EntityTypeBuilder<PackageLocation> builder)
        {
            builder.ToTable("PackageLocation");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.LocationId);
            builder.HasIndex(x => x.PackageId);

            // Foreign Keys
            builder.HasOne(bp => bp.Location)
                .WithMany(b => b.PackageLocations)
                .HasForeignKey(bp => bp.LocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(bp => bp.Package)
                .WithMany(p => p.PackageLocations)
                .HasForeignKey(bp => bp.PackageId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
