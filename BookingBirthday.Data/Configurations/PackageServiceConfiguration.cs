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
    public class ServiceserviceConfiguration : IEntityTypeConfiguration<PackageService>
    {
        public void Configure(EntityTypeBuilder<PackageService> builder)
        {
            builder.ToTable("PackageService");

            // Primary Key
            builder.HasKey(ps => new { ps.PackageId, ps.ServiceId });

            // Foreign Key
            builder.HasOne(ps => ps.Package)
                .WithMany(x => x.PackageService)
                .HasForeignKey(ps => ps.PackageId);

            builder.HasOne(ps => ps.Service)
                .WithMany(x => x.PackageService)
                .HasForeignKey(ps => ps.ServiceId);
        }
    }
}
