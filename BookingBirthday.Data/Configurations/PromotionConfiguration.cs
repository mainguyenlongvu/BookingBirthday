using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotion");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.FromDate).IsRequired();
            builder.Property(x => x.ToDate).IsRequired();
            builder.Property(x => x.DiscountPercent).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(Status.Accept);

            // 1:M relationship with Host
            builder.HasOne<Host>(x => x.Host)
                .WithMany(x => x.Promotions)
                .HasForeignKey(x => x.HostId);
        }
    }
}
