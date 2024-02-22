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
    public class CartPackageConfiguration : IEntityTypeConfiguration<CartPackage>
    {
        public void Configure(EntityTypeBuilder<CartPackage> builder)
        {
            builder.ToTable("CartPackage");

            // Primary Key
            builder.HasKey(cp => new { cp.CartId, cp.PackageId });

            // Foreign Key
            builder.HasOne(cp => cp.Cart)
                .WithMany(x => x.CartPackage)
                .HasForeignKey(cp => cp.CartId);

            builder.HasOne(cp => cp.Package)
                .WithMany(x => x.CartServices)
                .HasForeignKey(cp => cp.PackageId);
        }
    }
}
