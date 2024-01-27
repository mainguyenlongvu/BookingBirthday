using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties
            builder.Property(x => x.Detail).IsUnicode();

            // 1:M relationship with Host
            builder.HasOne<Host>(x => x.Host)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.HostId);

            // 1:M relationship with Guest
            builder.HasOne<Guest>(x => x.Guest)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.GuestId);
        }
    }
}
