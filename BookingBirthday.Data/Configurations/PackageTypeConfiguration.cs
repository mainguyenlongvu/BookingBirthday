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
    public class PackageTypeConfiguration : IEntityTypeConfiguration<PackageType>
    {
        public void Configure(EntityTypeBuilder<PackageType> builder)
        {
            builder.ToTable("PackageType");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties

            builder.Property(x => x.Name).IsUnicode().IsRequired();



        }
    }
}
