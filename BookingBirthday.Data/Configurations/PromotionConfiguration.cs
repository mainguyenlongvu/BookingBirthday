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
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.FromDate).IsRequired();
            builder.Property(x => x.ToDate).IsRequired();
            builder.Property(x => x.DiscountPercent).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasIndex(x => x.UserId);
            // 1:M relationship with Host
            builder.HasOne<User>(x => x.User)
                .WithMany(x => x.Promotions)
                .HasForeignKey(x => x.UserId);
        }
    }
}
