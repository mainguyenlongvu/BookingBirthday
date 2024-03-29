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
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Detail).IsUnicode().IsRequired();
            builder.Property(x => x.Note).IsUnicode().IsRequired();
            builder.Property(x => x.image_url);
            builder.Property(x => x.Host_name).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(b => b.ThemeId);

         
            // 1:M relationship with User
            builder.HasOne(x => x.User)
              .WithMany(b => b.Packages)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.NoAction);
           
            
            // 1:M relationship with Theme
            builder.HasOne(x => x.Theme)
              .WithMany(b => b.Package)
              .HasForeignKey(x => x.ThemeId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
